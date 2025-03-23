// API\Controllers\AuthController.cs
using Microsoft.AspNetCore.Mvc;
using Application.DTOs;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly JwtTokenService _jwtTokenService;

    public AuthController(JwtTokenService jwtTokenService)
    {
        _jwtTokenService = jwtTokenService;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] UserLoginDto loginDto)
    {
        // Demo: Hardcoded user (in production, fetch from DB)
        if (loginDto.Username == "admin" && loginDto.Password == "password")
        {
            var token = _jwtTokenService.GenerateToken(loginDto.Username, "Admin");
            return Ok(new { Token = token });
        }

        return Unauthorized("Invalid credentials");
    }
}
