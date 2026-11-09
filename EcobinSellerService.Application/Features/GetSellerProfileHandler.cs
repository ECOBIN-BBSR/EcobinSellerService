using System;
using EcobinSellerService.Application.DTOs.Request;
using EcobinSellerService.Application.Interfaces;

namespace EcobinSellerService.Application.Features;

public class GetSellerProfileHandler
{
    private readonly ISellerRepository _sellerRepository;

    public GetSellerProfileHandler(ISellerRepository sellerRepository)
    {
        _sellerRepository = sellerRepository;
    }

    public async Task<SellerProfileResponseDto?> Handle(
        long sellerId,
        CancellationToken cancellationToken)
    {
        var seller = await _sellerRepository.GetByIdAsync(
            sellerId,
            cancellationToken);

        if (seller == null)
            return null;

        return new SellerProfileResponseDto
        {
            Id = seller.Id,
            Name = seller.Name,
            Email = seller.Email,
            Phone = seller.Phone,
            Address = seller.Address
        };
    }
}
