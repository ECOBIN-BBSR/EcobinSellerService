using System;

namespace EcobinSellerService.Application.DTOs.Request;

public class LoginSellerResult
{
    public LoginSellerResponseDto Response { get; set; } = new();

    public string RefreshToken { get; set; } = string.Empty;
}
