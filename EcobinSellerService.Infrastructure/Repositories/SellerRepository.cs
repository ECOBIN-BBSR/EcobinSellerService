using EcobinSellerService.Application.Interfaces;
using EcobinSellerService.Domain.Entities;
using EcobinSellerService.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace EcobinSellerService.Infrastructure.Repositories
{
    public class SellerRepository : ISellerRepository
    {
        private readonly ApplicationDbContext _context;

        public SellerRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> EmailExistsAsync(
            string email,
            CancellationToken cancellationToken = default)
        {
            return await _context.Sellers
                .AnyAsync(
                    s => s.Email == email,
                    cancellationToken);
        }

        public async Task AddAsync(
            Seller seller,
            CancellationToken cancellationToken = default)
        {
            await _context.Sellers.AddAsync(
                seller, cancellationToken);
        }

        public async Task SaveChangesAsync(
            CancellationToken cancellationToken = default)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<Seller?> GetByEmailAsync(
            string email, 
            CancellationToken cancellationToken)
        {
            return await _context.Sellers
                .FirstOrDefaultAsync(
                    s => s.Email == email,
                    cancellationToken);
        }

        public async Task<Seller?> GetByIdAsync(
            long id,
            CancellationToken cancellationToken = default)
        {
            return await _context.Sellers
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    s => s.Id == id,
                    cancellationToken);
        }
    }
}
