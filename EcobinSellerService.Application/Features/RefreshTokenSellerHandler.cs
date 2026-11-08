
using EcobinSellerService.Application.DTOs.Request;
using EcobinSellerService.Application.Interfaces;
using EcobinSellerService.Domain.Entities;

namespace EcobinSellerService.Application.Features;

public class RefreshSellerTokenHandler
{
    private readonly ISellerRefreshTokenRepository _refreshTokenRepository;
    private readonly IRefreshTokenService _refreshTokenService;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public RefreshSellerTokenHandler(
        ISellerRefreshTokenRepository refreshTokenRepository,
        IRefreshTokenService refreshTokenService,
        IJwtTokenGenerator jwtTokenGenerator)
    {
        _refreshTokenRepository = refreshTokenRepository;
        _refreshTokenService = refreshTokenService;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<LoginSellerResult> Handle(
        string refreshToken,
        CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;

        // Hash the token received from the browser
        var tokenHash = _refreshTokenService.HashToken(refreshToken);

        // Find the token and its seller
        var existingToken = await _refreshTokenRepository.GetByHashAsync(
            tokenHash,
            cancellationToken);

        if (existingToken == null)
        {
            throw new UnauthorizedAccessException(
                "Invalid refresh token.");
        }

        // A previously rotated token is being reused.
        // Revoke all tokens belonging to that login session.
        if (existingToken.RevokedAt != null)
        {
            await _refreshTokenRepository.RevokeFamilyAsync(
                existingToken.TokenFamily,
                now,
                cancellationToken);

            throw new UnauthorizedAccessException(
                "Refresh token reuse detected. Please log in again.");
        }

        if (existingToken.ExpiresAt <= now)
        {
            throw new UnauthorizedAccessException(
                "Refresh token has expired. Please log in again.");
        }

        // Generate replacement refresh token
        var newRefreshToken = _refreshTokenService.GenerateToken();

        var replacementToken = new SellerRefreshToken
        {
            SellerId = existingToken.SellerId,
            TokenHash = _refreshTokenService.HashToken(newRefreshToken),
            TokenFamily = existingToken.TokenFamily,
            ExpiresAt = now.AddDays(7),

            CreatedAt = now,
            CreatedBy = existingToken.Seller.Email
        };

        // Atomically rotate the old token
        var rotated = await _refreshTokenRepository.RotateAsync(
            existingToken.Id,
            replacementToken,
            now,
            cancellationToken);

        if (!rotated)
        {
            await _refreshTokenRepository.RevokeFamilyAsync(
                existingToken.TokenFamily,
                now,
                cancellationToken);

            throw new UnauthorizedAccessException(
                "Refresh token is no longer valid. Please log in again.");
        }

        // Generate a new access token
        var generatedToken = _jwtTokenGenerator.GenerateToken(
            existingToken.Seller);

        return new LoginSellerResult
        {
            Response = new LoginSellerResponseDto
            {
                AccessToken = generatedToken.Token,
                ExpiresAtUtc = generatedToken.ExpiresAtUtc,
                SellerId = existingToken.Seller.Id.ToString(),
                Name = existingToken.Seller.Name,
                Email = existingToken.Seller.Email
            },
            RefreshToken = newRefreshToken
        };
    }
}
