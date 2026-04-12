using ApartmentAz.BLL.DTOs.Auth;
using ApartmentAz.BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApartmentAz.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(new AuthResultDto
            {
                Succeeded = false,
                ErrorMessage = string.Join("; ", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage))
            });

        var result = await _authService.RegisterAsync(dto);

        if (!result.IsSuccess)
            return BadRequest(new AuthResultDto
            {
                Succeeded = false,
                ErrorMessage = result.ErrorMessage
            });

        return Ok(result.Data);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(new AuthResultDto
            {
                Succeeded = false,
                ErrorMessage = string.Join("; ", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage))
            });

        var result = await _authService.LoginAsync(dto);

        if (!result.IsSuccess)
            return Unauthorized(new AuthResultDto
            {
                Succeeded = false,
                ErrorMessage = result.ErrorMessage
            });

        return Ok(result.Data);
    }

    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        await _authService.LogoutAsync();
        return NoContent();
    }
}
