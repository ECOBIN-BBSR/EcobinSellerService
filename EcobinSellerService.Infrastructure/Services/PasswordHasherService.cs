using EcobinSellerService.Application.Interfaces;
using EcobinSellerService.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace EcobinSellerService.Infrastructure.Services
{
    public class PasswordHasherService : IPasswordHasher
    {
        private readonly PasswordHasher<Seller> _hasher = new();
        
        public string HashPassword(string password)
        {
            return _hasher.HashPassword(null!, password);
        }
        
        public bool VerifyPassword(
            Seller seller,
            string hashedPassword,
            string providedPassword)
        {
            var result = _hasher.VerifyHashedPassword(
                seller,
                hashedPassword,
                providedPassword);

            return result != PasswordVerificationResult.Failed;
        }
    }
}
