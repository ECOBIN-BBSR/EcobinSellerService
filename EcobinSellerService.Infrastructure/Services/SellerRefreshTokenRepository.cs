
using EcobinSellerService.Application.Interfaces;
using EcobinSellerService.Domain.Entities;
using EcobinSellerService.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace EcobinSellerService.Infrastructure.Repositories;

public class SellerRefreshTokenRepository
    : ISellerRefreshTokenRepository
{
    private readonly ApplicationDbContext _context;

    public SellerRefreshTokenRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(
        SellerRefreshToken refreshToken,
        CancellationToken cancellationToken)
    {
        await _context.SellerRefreshTokens.AddAsync(
            refreshToken,
            cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<SellerRefreshToken?> GetByHashAsync(
        string tokenHash,
        CancellationToken cancellationToken)
    {
        return await _context.SellerRefreshTokens
            .Include(x => x.Seller)
            .AsNoTracking()
            .FirstOrDefaultAsync(
                x => x.TokenHash == tokenHash,
                cancellationToken);
    }

    public async Task<bool> RotateAsync(
        long currentTokenId,
        SellerRefreshToken replacementToken,
        DateTime now,
        CancellationToken cancellationToken)
    {
        await using var transaction =
            await _context.Database.BeginTransactionAsync(
                cancellationToken);

        await _context.SellerRefreshTokens.AddAsync(
            replacementToken,
            cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        var updatedRows = await _context.SellerRefreshTokens
            .Where(x =>
                x.Id == currentTokenId &&
                x.RevokedAt == null &&
                x.ExpiresAt > now)
            .ExecuteUpdateAsync(
                setters => setters
                    .SetProperty(x => x.RevokedAt, now)
                    .SetProperty(
                        x => x.ReplacedByTokenId,
                        replacementToken.Id),
                cancellationToken);

        if (updatedRows == 0)
        {
            await transaction.RollbackAsync(cancellationToken);
            return false;
        }

        await transaction.CommitAsync(cancellationToken);

        return true;
    }

    public async Task RevokeFamilyAsync(
        string tokenFamily,
        DateTime revokedAt,
        CancellationToken cancellationToken)
    {
        await _context.SellerRefreshTokens
            .Where(x =>
                x.TokenFamily == tokenFamily &&
                x.RevokedAt == null)
            .ExecuteUpdateAsync(
                setters => setters.SetProperty(
                    x => x.RevokedAt,
                    revokedAt),
                cancellationToken);
    }
}
