using System;
using EcobinSellerService.Domain.Entities;

namespace EcobinSellerService.Application.Interfaces;

public interface IJwtTokenGenerator
{
    string Generatetoken(Seller seller);
}
