using System;

namespace EcobinSellerService.Application.Interfaces;

public interface IRefreshTokenService
{
    string GenerateToken();

    string HashToken(string token);
}
