using System;

namespace EcobinSellerService.Application.DTOs.Request;

public class LoginSellerResponseDto
{
    public string Token { get; set; } = string.Empty;

    public string TokenType { get; set; } = "Bearer";

    public string SellerId { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;
}
