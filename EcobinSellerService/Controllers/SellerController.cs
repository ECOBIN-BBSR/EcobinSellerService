using EcobinSellerService.Application.Features;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EcobinSellerService.Controllers;

[ApiController]
[Route("api/sellers")]
[Authorize]
public class SellerController : ControllerBase
{
    private readonly GetSellerProfileHandler _profileHandler;

    public SellerController(GetSellerProfileHandler profileHandler)
    {
        _profileHandler = profileHandler;
    }

    [HttpGet("me")]
    public async Task<IActionResult> GetMyProfile(
        CancellationToken cancellationToken)
    {
        var sellerIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)
                           ?? User.FindFirst("sub");

        if (sellerIdClaim == null ||
            !long.TryParse(sellerIdClaim.Value, out var sellerId))
        {
            return Unauthorized(new
            {
                message = "Invalid seller identity."
            });
        }

        var profile = await _profileHandler.Handle(
            sellerId,
            cancellationToken);

        if (profile == null)
        {
            return NotFound(new
            {
                message = "Seller profile not found."
            });
        }

        return Ok(profile);
    }
}