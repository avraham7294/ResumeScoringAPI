using Microsoft.AspNetCore.Mvc;
using Application.DTOs;

namespace AIResumeScoringAPI.API.Controllers
{
    /// <summary>
    /// Controller responsible for user authentication and token generation.
    /// </summary>
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly JwtTokenService _jwtTokenService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthController"/> class.
        /// </summary>
        /// <param name="jwtTokenService">JWT token generation service.</param>
        public AuthController(JwtTokenService jwtTokenService)
        {
            _jwtTokenService = jwtTokenService;
        }

        /// <summary>
        /// Authenticates the user and returns a JWT token if credentials are valid.
        /// </summary>
        /// <param name="loginDto">User login details (username and password).</param>
        /// <returns>JWT token on success, Unauthorized on failure.</returns>
        [HttpPost("login")]
        public IActionResult Login([FromBody] UserLoginDto loginDto)
        {
            // ✅ In production, this should be replaced by proper user validation from a database.
            if (loginDto.Username == "admin" && loginDto.Password == "password")
            {
                var token = _jwtTokenService.GenerateToken(loginDto.Username, "Admin");
                return Ok(new { Token = token });
            }

            return Unauthorized("Invalid credentials");
        }
    }
}
