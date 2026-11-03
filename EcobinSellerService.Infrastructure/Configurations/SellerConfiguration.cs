using EcobinSellerService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace EcobinSellerService.Infrastructure.Configurations
{
    public class SellerConfiguration : IEntityTypeConfiguration<Seller>
    {
        public void Configure(EntityTypeBuilder<Seller> builder)
        {
            builder.ToTable("sellers");

            builder.HasKey(s => s.Id);

            builder.Property(s => s.Name)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(s => s.Address)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(s => s.Email)
                .IsRequired()
                .HasMaxLength(255);

            builder.HasIndex(s => s.Email)
                .IsUnique();

            builder.Property(s => s.Phone)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(s => s.PasswordHash)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(s => s.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Property(s => s.CreatedBy)
                .IsRequired(false);

            builder.Property(s => s.ModifiedAt)
                .IsRequired(false);

            builder.Property(s => s.ModifiedBy)
                .IsRequired(false);
        }
    }
}
