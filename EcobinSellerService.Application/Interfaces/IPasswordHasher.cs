using System;
using System.Collections.Generic;
using System.Text;
using EcobinSellerService.Domain.Entities;

namespace EcobinSellerService.Application.Interfaces
{
    public interface IPasswordHasher
    {
        string HashPassword(string password);

        bool VerifyPassword(
            Seller seller,
            string hashedPassword,
            string providedPassword);
    }
}