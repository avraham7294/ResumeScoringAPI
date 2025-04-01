using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

/// <summary>
/// Service responsible for generating JWT tokens for authenticated users.
/// </summary>
public class JwtTokenService
{
    private readonly IConfiguration _configuration;

    /// <summary>
    /// Initializes a new instance of the <see cref="JwtTokenService"/> class.
    /// </summary>
    /// <param name="configuration">Application configuration to read JWT settings.</param>
    public JwtTokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    /// <summary>
    /// Generates a JWT token for a given user.
    /// </summary>
    /// <param name="username">Username of the authenticated user.</param>
    /// <param name="role">Role of the user (e.g., Admin, User).</param>
    /// <returns>Generated JWT token as a string.</returns>
    public string GenerateToken(string username, string role)
    {
        // 🔸 Read JWT settings from configuration
        var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);
        var issuer = _configuration["Jwt:Issuer"];

        // 🔸 Define claims to include in the token
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, username),
            new Claim(ClaimTypes.Role, role)
        };

        // 🔸 Create signing credentials using the secret key
        var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

        // 🔸 Create the JWT token
        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: issuer,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2), // Token expiration time
            signingCredentials: credentials
        );

        // 🔸 Return the serialized token string
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
