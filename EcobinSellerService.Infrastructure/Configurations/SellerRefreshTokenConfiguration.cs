using EcobinSellerService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EcobinSellerService.Infrastructure.Configurations;

public class SellerRefreshTokenConfiguration
    : IEntityTypeConfiguration<SellerRefreshToken>
{
    public void Configure(
        EntityTypeBuilder<SellerRefreshToken> builder)
    {
        builder.ToTable("seller_refresh_tokens");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.TokenHash)
            .IsRequired()
            .HasMaxLength(64);

        builder.HasIndex(x => x.TokenHash)
            .IsUnique();

        builder.Property(x => x.TokenFamily)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasIndex(x => x.TokenFamily);

        builder.Property(x => x.ExpiresAt)
            .IsRequired();

        builder.Property(x => x.RevokedAt);

        builder.Property(x => x.ModifiedBy)
            .IsRequired(false);

        builder.HasOne(x => x.Seller)
            .WithMany()
            .HasForeignKey(x => x.SellerId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}