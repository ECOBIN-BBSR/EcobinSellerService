using System;
using System.Collections.Generic;
using System.Text;

namespace EcobinSellerService.Application.Interfaces
{
    public interface IPasswordHasher
    {
        string HashPassword(string password);
    }
}
