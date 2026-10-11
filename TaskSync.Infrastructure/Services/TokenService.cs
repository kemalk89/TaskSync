using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

using TaskSync.Domain.Auth;
using TaskSync.Domain.User;

namespace TaskSync.Infrastructure.Services;

public class TokenService : ITokenService
{
    private readonly IConfiguration _config;

    public TokenService(IConfiguration config)
    {
        _config = config;
    }

    public TokenResult GenerateJwt(User user)
    {
        var issuer = _config.GetValue("LocalAuth:ValidIssuer", string.Empty);
        var lifetimeInMins = _config.GetValue("LocalAuth:AccessTokenLifetimeInMinutes", 15);
        var issuedAt = DateTime.UtcNow;
        var expiresIn = issuedAt.AddMinutes(lifetimeInMins);
        
        var claims = new[]
        {
            new Claim("id", user.Id.ToString()),
            new Claim("username", user.Email),
            new Claim("email", user.Email),
            new Claim(JwtRegisteredClaimNames.Iat, EpochTime.GetIntDate(issuedAt.ToUniversalTime()).ToString())
        };
        
        string secretKey = _config.GetValue("LocalAuth:JwtSecret", string.Empty);
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
        
        var jwt = new JwtSecurityToken(
            issuer: issuer,
            expires: expiresIn,
            signingCredentials: credentials,
            claims: claims);

        var result = new JwtSecurityTokenHandler().WriteToken(jwt);
        return new TokenResult
        {
            ExpiresIn = EpochTime.GetIntDate(expiresIn.ToUniversalTime()), 
            Jwt = result
        };
    }

    public bool VerifyJwt(string jwt)
    {
        throw new NotImplementedException();
    }
}
