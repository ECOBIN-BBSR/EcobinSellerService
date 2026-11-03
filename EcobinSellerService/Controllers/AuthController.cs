using EcobinSellerService.Application.DTOs.Request;
using EcobinSellerService.Application.Features;
using Microsoft.AspNetCore.Mvc;

namespace EcobinSellerService.Controllers
{
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly RegisterSellerHandler _registerHandler;

        public AuthController(RegisterSellerHandler registerHandler)
        {
            _registerHandler = registerHandler;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(
            [FromBody] RegisterSellerRequestDto request,
            CancellationToken cancellationToken)
        {
            try
            {
                var result = await _registerHandler.Handle(
                    request, cancellationToken);

                return Created(
                    $"/api/sellers/{result.Id}",
                    result);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }
    }
}
