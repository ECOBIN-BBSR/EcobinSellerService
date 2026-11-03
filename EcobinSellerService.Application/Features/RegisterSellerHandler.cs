using EcobinSellerService.Application.DTOs.Request;
using EcobinSellerService.Application.Interfaces;
using EcobinSellerService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace EcobinSellerService.Application.Features
{
    public class RegisterSellerHandler
    {
        private readonly ISellerRepository _sellerRepository;
        private readonly IPasswordHasher _passwordHasher;

        public RegisterSellerHandler(
            ISellerRepository sellerRepository,
            IPasswordHasher passwordHasher)
        {
            _sellerRepository = sellerRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task<RegisterSellerResponseDto> Handle(
            RegisterSellerRequestDto request,
            CancellationToken cancellationToken = default)
        {
            string email = request.Email.Trim().ToLowerInvariant();

            // Check whether seller already exists
            bool emailExists = await _sellerRepository.EmailExistsAsync(
                email, cancellationToken);

            if (emailExists)
            {
                throw new InvalidOperationException(
                    "A seller with this email already exists.");
            }

            // Hash password
            string passwordHash = _passwordHasher.HashPassword(
                request.Password);

            // Create domain entity
            Seller seller = new Seller
            {
                Name = request.Name.Trim(),
                Address = request.Address.Trim(),
                Email = email,
                Phone = request.Phone.Trim(),
                CreatedBy = email,
                PasswordHash = passwordHash
            };

            // Save seller
            await _sellerRepository.AddAsync(
                seller, cancellationToken);

            await _sellerRepository.SaveChangesAsync(
                cancellationToken);

            // Return safe response
            return new RegisterSellerResponseDto
            {
                Id = seller.Id,
                Name = seller.Name,
                Email = seller.Email,
                Phone = seller.Phone,
                CreatedAt = seller.CreatedAt
            };
        }
    }
}
