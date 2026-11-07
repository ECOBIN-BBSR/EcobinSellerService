using System;

namespace EcobinSellerService.Application.DTOs.Request;

public class LoginSellerRequestDto
{
    public string Email { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;
}
