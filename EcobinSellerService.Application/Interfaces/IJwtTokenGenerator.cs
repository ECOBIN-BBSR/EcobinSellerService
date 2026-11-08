using System;
using EcobinSellerService.Application.DTOs.Request;
using EcobinSellerService.Domain.Entities;

namespace EcobinSellerService.Application.Interfaces;

public interface IJwtTokenGenerator
{
    GeneratedAccessToken GenerateToken(Seller seller);
}
