
using EcobinSellerService.Application.DTOs.Request;
using EcobinSellerService.Application.Interfaces;
using EcobinSellerService.Domain.Entities;

namespace EcobinSellerService.Application.Features;

public class LoginSellerHandler
{
    private readonly ISellerRepository _sellerRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IRefreshTokenService _refreshTokenService;
    private readonly ISellerRefreshTokenRepository _refreshTokenRepository;

    public LoginSellerHandler(
        ISellerRepository sellerRepository,
        IPasswordHasher passwordHasher,
        IJwtTokenGenerator jwtTokenGenerator,
        IRefreshTokenService refreshTokenService,
        ISellerRefreshTokenRepository refreshTokenRepository)
    {
        _sellerRepository = sellerRepository;
        _passwordHasher = passwordHasher;
        _jwtTokenGenerator = jwtTokenGenerator;
        _refreshTokenService = refreshTokenService;
        _refreshTokenRepository = refreshTokenRepository;
    }

    public async Task<LoginSellerResult> Handle(
        LoginSellerRequestDto request,
        CancellationToken cancellationToken)
    {
        var email = request.Email.Trim().ToLowerInvariant();

        var seller = await _sellerRepository.GetByEmailAsync(
            email,
            cancellationToken);

        if (seller == null)
            throw new UnauthorizedAccessException(
                "Invalid email or password.");

        var isPasswordValid = _passwordHasher.VerifyPassword(
            seller,
            seller.PasswordHash,
            request.Password);

        if (!isPasswordValid)
            throw new UnauthorizedAccessException(
                "Invalid email or password.");

        // Generate access token
        var generatedToken = _jwtTokenGenerator.GenerateToken(seller);

        // Generate refresh token
        var refreshToken = _refreshTokenService.GenerateToken();

        var refreshTokenEntity = new SellerRefreshToken
        {
            SellerId = seller.Id,
            TokenHash = _refreshTokenService.HashToken(refreshToken),
            TokenFamily = Guid.NewGuid().ToString(),
            ExpiresAt = DateTime.UtcNow.AddDays(7),

            CreatedAt = DateTime.UtcNow,
            CreatedBy = seller.Email
};

        // Save hashed refresh token
        await _refreshTokenRepository.AddAsync(
            refreshTokenEntity,
            cancellationToken);

        return new LoginSellerResult
        {
            Response = new LoginSellerResponseDto
            {
                AccessToken = generatedToken.Token,
                ExpiresAtUtc = generatedToken.ExpiresAtUtc,
                SellerId = seller.Id.ToString(),
                Name = seller.Name,
                Email = seller.Email
            },
            RefreshToken = refreshToken
        };
    }
}
