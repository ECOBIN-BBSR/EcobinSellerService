using EcobinSellerService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace EcobinSellerService.Application.Interfaces
{
    public interface ISellerRepository
    {
        Task<bool> EmailExistsAsync(
            string email,
            CancellationToken cancellationToken = default);

        Task AddAsync(
            Seller seller,
            CancellationToken cancellationToken = default);

        Task SaveChangesAsync(
            CancellationToken cancellationToken = default);

        Task<Seller?> GetByEmailAsync(
            string email, 
            CancellationToken cancellationToken);
    }
}
