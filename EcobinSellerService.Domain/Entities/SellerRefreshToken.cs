using System;

namespace EcobinSellerService.Domain.Entities;

public class SellerRefreshToken : Base
{
    public long Id { get; set; }

    public long SellerId { get; set; }

    // SHA-256 hash of the actual refresh token
    public string TokenHash { get; set; } = string.Empty;

    // Groups tokens belonging to the same login session
    public string TokenFamily { get; set; } = string.Empty;

    public DateTime ExpiresAt { get; set; }

    public DateTime? RevokedAt { get; set; }

    // Identifies the replacement token after rotation
    public long? ReplacedByTokenId { get; set; }

    public Seller Seller { get; set; } = null!;
}
