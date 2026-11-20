using System.Security.Claims;
using System.Text.Encodings.Web;

using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace TaskSync.Tests.IntegrationTests;

/// <summary>
/// Test authentication handler for bypassing real authentication.
/// </summary>
public class TestAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private readonly TestJwtGenerator _testJwtGenerator;

    public TestAuthHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        TestJwtGenerator testJwtGenerator) 
        : base(options, logger, encoder)
    {
        _testJwtGenerator = testJwtGenerator;
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        // Check if authorization header exists
        if (!Request.Headers.ContainsKey("Authorization"))
        {
            return Task.FromResult(AuthenticateResult.Fail("Missing Authorization header"));
        }

        var authHeader = Request.Headers["Authorization"].ToString();
        if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
        {
            return Task.FromResult(AuthenticateResult.Fail("Invalid Authorization header"));
        }

        try
        {
            // Decode the auth data from the header
            var encodedData = authHeader.Substring("Bearer ".Length);
            var data = _testJwtGenerator.Decode(encodedData);
            var identity = new ClaimsIdentity(data.Claims, "TestScheme");
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
    public string? Email { get; set; }
}