using EcobinSellerService.Application.DTOs.Request;
using EcobinSellerService.Application.Features;
using Microsoft.AspNetCore.Mvc;

namespace EcobinSellerService.Controllers
{
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly RegisterSellerHandler _registerHandler;
        private readonly LoginSellerHandler _loginHandler;
        public AuthController(
            RegisterSellerHandler registerHandler, 
            LoginSellerHandler loginSellerHandler)
        {
            _registerHandler = registerHandler;
            _loginHandler = loginSellerHandler;
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

        [HttpPost("login")]
        public async Task<IActionResult> Login(
            [FromBody] LoginSellerRequestDto request,
            CancellationToken cancellationToken)
        {
            try
            {
                var result = await _loginHandler.Handle(
                    request, cancellationToken);

                return Ok(result);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new
                {
                    message = ex.Message
                });
            }
        }
    }
}
