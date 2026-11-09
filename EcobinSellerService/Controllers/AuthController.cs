
using EcobinSellerService.Application.DTOs.Request;
using EcobinSellerService.Application.Features;
using Microsoft.AspNetCore.Mvc;

namespace EcobinSellerService.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly RegisterSellerHandler _registerHandler;
    private readonly LoginSellerHandler _loginHandler;
    private readonly RefreshSellerTokenHandler _refreshHandler;
    private readonly LogoutSellerHandler _logoutHandler;

    public AuthController(
        RegisterSellerHandler registerHandler,
        LoginSellerHandler loginSellerHandler,
        RefreshSellerTokenHandler refreshHandler,
        LogoutSellerHandler logoutSellerHandler)
    {
        _registerHandler = registerHandler;
        _loginHandler = loginSellerHandler;
        _refreshHandler = refreshHandler;
        _logoutHandler = logoutSellerHandler;
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

            return Created($"/api/sellers/{result.Id}", result);
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

            Response.Cookies.Append(
                "refreshToken",
                result.RefreshToken,
                new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Lax,
                    Expires = DateTimeOffset.UtcNow.AddDays(7),
                    Path = "/api/auth"
                });

            return Ok(result.Response);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
    }


    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh(
        CancellationToken cancellationToken)
    {
        var refreshToken = Request.Cookies["refreshToken"];

        if (string.IsNullOrEmpty(refreshToken))
        {
            return Unauthorized(new
            {
                message = "Refresh token is missing. Please log in again."
            });
        }

        try
        {
            var result = await _refreshHandler.Handle(
                refreshToken,
                cancellationToken);

            Response.Cookies.Append(
                "refreshToken",
                result.RefreshToken,
                new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Lax,
                    Expires = DateTimeOffset.UtcNow.AddDays(7),
                    Path = "/api/auth"
                });

            return Ok(result.Response);
        }
        catch (UnauthorizedAccessException ex)
        {
            Response.Cookies.Delete(
                "refreshToken",
                new CookieOptions
                {
                    Secure = true,
                    SameSite = SameSiteMode.Lax,
                    Path = "/api/auth"
                });

            return Unauthorized(new
            {
                message = ex.Message
            });
        }
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout(
        CancellationToken cancellationToken)
    {
        var refreshToken = Request.Cookies["refreshToken"];

        if (!string.IsNullOrEmpty(refreshToken))
        {
            await _logoutHandler.Handle(
                refreshToken,
                cancellationToken);
        }

        Response.Cookies.Delete(
            "refreshToken",
            new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Lax,
                Path = "/api/auth"
            });

        return Ok(new
        {
            message = "Logged out successfully."
        });
    }

}
