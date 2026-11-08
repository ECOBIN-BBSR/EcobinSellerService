using System;
using EcobinSellerService.Domain.Entities;

namespace EcobinSellerService.Application.Interfaces;

public interface ISellerRefreshTokenRepository
{
     Task AddAsync(
        SellerRefreshToken refreshToken,
        CancellationToken cancellationToken);

    Task<SellerRefreshToken?> GetByHashAsync(
        string tokenHash,
        CancellationToken cancellationToken);

    Task<bool> RotateAsync(
        long currentTokenId,
        SellerRefreshToken replacementToken,
        DateTime now,
        CancellationToken cancellationToken);

    Task RevokeFamilyAsync(
        string tokenFamily,
        DateTime revokedAt,
        CancellationToken cancellationToken);
}
