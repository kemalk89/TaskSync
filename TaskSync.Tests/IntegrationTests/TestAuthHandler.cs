using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;

using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace TaskSync.Tests.IntegrationTests;

/// <summary>
/// Test authentication handler for bypassing authentication in tests.
/// </summary>
public class TestAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public TestAuthHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder) 
        : base(options, logger, encoder)
    {
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        // Check if authorization header exists
        if (!Request.Headers.ContainsKey("Authorization"))
        {
            return Task.FromResult(AuthenticateResult.Fail("Missing Authorization header"));
        }

        var authHeader = Request.Headers["Authorization"].ToString();
        if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("TestScheme "))
        {
            return Task.FromResult(AuthenticateResult.Fail("Invalid Authorization header"));
        }

        try
        {
            // Decode the auth data from the header
            var encodedData = authHeader.Substring("TestScheme ".Length);
            var json = Encoding.UTF8.GetString(Convert.FromBase64String(encodedData));
            var authData = JsonSerializer.Deserialize<TestAuthData>(json);

            if (authData == null)
            {
                return Task.FromResult(AuthenticateResult.Fail("Invalid auth data"));
            }

            // Build claims from the auth data
            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, authData.UserName)
            };

            // Add role claims
            foreach (var role in authData.Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var identity = new ClaimsIdentity(claims, "TestScheme");
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, "TestScheme");

            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
        catch
        {
            return Task.FromResult(AuthenticateResult.Fail("Failed to parse auth data"));
        }
    }
}

/// <summary>
/// DTO for test authentication.
/// </summary>
public class TestAuthData
{
    public string[] Roles { get; set; } = [];
    public string UserName { get; set; } = string.Empty;
}