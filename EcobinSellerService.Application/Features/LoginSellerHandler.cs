using System;
using EcobinSellerService.Application.DTOs.Request;
using EcobinSellerService.Application.Interfaces;

namespace EcobinSellerService.Application.Features;

public class LoginSellerHandler
{
    private readonly ISellerRepository _sellerRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public LoginSellerHandler(
        ISellerRepository sellerRepository,
        IPasswordHasher passwordHasher,
        IJwtTokenGenerator jwtTokenGenerator)
    {
        _sellerRepository = sellerRepository;
        _passwordHasher = passwordHasher;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<LoginSellerResponseDto> Handle(
        LoginSellerRequestDto request,
        CancellationToken cancellationToken)
    {
        var email = request.Email.Trim().ToLowerInvariant();

        var seller = await _sellerRepository.GetByEmailAsync(
            email,
            cancellationToken);

        if (seller == null)
        {
            throw new UnauthorizedAccessException(
                "Invalid email or password.");
        }

        var isPasswordValid = _passwordHasher.VerifyPassword(
            seller,
            seller.PasswordHash,
            request.Password);

        if (!isPasswordValid)
        {
            throw new UnauthorizedAccessException(
                "Invalid email or password.");
        }

        var token = _jwtTokenGenerator.Generatetoken(seller);

        return new LoginSellerResponseDto
        {
            Token = token,
            SellerId = seller.Id.ToString(),
            Name = seller.Name,
            Email = seller.Email
        };
    }
}
