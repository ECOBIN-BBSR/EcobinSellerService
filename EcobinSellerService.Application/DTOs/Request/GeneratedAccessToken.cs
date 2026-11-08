using System;

namespace EcobinSellerService.Application.DTOs.Request;

public class GeneratedAccessToken
{
    public string Token { get; set; } = string.Empty;

    public DateTime ExpiresAtUtc { get; set; }
}
