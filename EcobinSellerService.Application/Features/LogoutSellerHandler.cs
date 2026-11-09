using System;
using EcobinSellerService.Application.Interfaces;

namespace EcobinSellerService.Application.Features;

public class LogoutSellerHandler
{
    private readonly ISellerRefreshTokenRepository _refreshTokenRepository;
    private readonly IRefreshTokenService _refreshTokenService;

    public LogoutSellerHandler(
        ISellerRefreshTokenRepository refreshTokenRepository,
        IRefreshTokenService refreshTokenService)
    {
        _refreshTokenRepository = refreshTokenRepository;
        _refreshTokenService = refreshTokenService;
    }

    public async Task Handle(
        string refreshToken,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(refreshToken))
            return;

        var tokenHash = _refreshTokenService.HashToken(refreshToken);

        var storedToken = await _refreshTokenRepository.GetByHashAsync(
            tokenHash,
            cancellationToken);

        if (storedToken == null || storedToken.RevokedAt != null)
            return;

        await _refreshTokenRepository.RevokeFamilyAsync(
            storedToken.TokenFamily,
            DateTime.UtcNow,
            cancellationToken);
    }
}
