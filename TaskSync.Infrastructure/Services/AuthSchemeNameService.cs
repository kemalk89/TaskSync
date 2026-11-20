using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.Net.Http.Headers;

namespace TaskSync.Infrastructure.Services;

public class AuthSchemeNameService
{
    public const string AuthSchemeNameAuth0 = "Auth0";
    
    /// <summary>
    /// If Authorization header exists, this method will read the Bearer token to understand whether it is JWT or
    /// opaque token.
    /// If no Authorization header exists, this method will return null.
    /// If Authorization header exists but no Bearer format, this method will return null as well.
    /// </summary>
    public static string GetAuthSchemeNameByRequest(HttpContext context, IConfiguration config)
    {
        string? authorization = context.Request.Headers[HeaderNames.Authorization];
        if (string.IsNullOrWhiteSpace(authorization))
        {
            // No Authorization header exists -> Default to Core Identity
            return IdentityConstants.BearerScheme;
        }

        if (!authorization.StartsWith("Bearer"))
        {
            // No Bearer format -> Default to Core Identity
            return IdentityConstants.BearerScheme;
        }
        
        var token = authorization["Bearer ".Length..].Trim(); 
        var jwtHandler = new JsonWebTokenHandler();
        // JWT (e.g. Auth0) or opaque tokens (Core Identity)
        if(jwtHandler.CanReadToken(token))
        {
            // JWT case
            var issuer = jwtHandler.ReadJsonWebToken(token).Issuer;
            var configIssuerAuth0 = config.GetValue<string>("Auth:ValidIssuer");
            if (!string.IsNullOrWhiteSpace(configIssuerAuth0) && configIssuerAuth0 == issuer)
            {
                return AuthSchemeNameAuth0;
            }
        }
        
        // opaque tokens (Core Identity) case
        return IdentityConstants.BearerScheme;
    }
}