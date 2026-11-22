using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace TaskSync.Tests.IntegrationTests;

public class TestJwtGenerator
{
    private readonly IConfiguration _config;
    private const string SigningKeySecret = "test-secret_integration-test-key-0123456789";

    public TestJwtGenerator(IConfiguration config)
    {
        _config = config;
    }

    public string Generate(string username, string? email, string[]? roles)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, username),
            // deprecated, should be removed
            new(ClaimTypes.Name, username)
        };
        
        if (!string.IsNullOrWhiteSpace(email))
        {
            claims.Add(new Claim(JwtRegisteredClaimNames.Email, email));   
        }

        claims.Add(new Claim("Roles", roles?.ToString() ?? string.Empty));
        
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SigningKeySecret));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        
        var token = new JwtSecurityToken(
            issuer: _config.GetValue<string>("Auth:ValidIssuer"),
            audience: _config.GetValue<string>("Auth:Audience"),
            claims,
            signingCredentials: credentials);
        
        var tokenHandler = new JwtSecurityTokenHandler();
        var jwt = tokenHandler.WriteToken(token);

        return jwt;
    }
    
    public JwtSecurityToken Decode(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        if (!tokenHandler.CanReadToken(token))
        {
            throw new ArgumentException("Invalid JWT format.", nameof(token));
        }

        return tokenHandler.ReadJwtToken(token);
    }
}