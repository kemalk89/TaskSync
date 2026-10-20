using System.Net;
using System.Net.Http.Json;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using TaskSync.Infrastructure;
using TaskSync.Infrastructure.Entities;

namespace TaskSync.Tests.IntegrationTests.Auth;

public class LoginTest : BaseIntegrationTest
{
    private const string Password = "Str0ng-P4ssw0rd!42";

    public LoginTest(IntegrationTestWebAppFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Login_ShouldReturnTokens_WhenCredentialsAreValid()
    {
        var email = await RegisterUserAsync();

        var response = await _client.PostAsJsonAsync("/login", new { Email = email, Password });

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var tokens = await response.Content.ReadFromJsonAsync<TokenResponse>();
        Assert.NotNull(tokens);
        Assert.Equal("Bearer", tokens.TokenType);
        Assert.False(string.IsNullOrEmpty(tokens.AccessToken));
        Assert.False(string.IsNullOrEmpty(tokens.RefreshToken));
        Assert.True(tokens.ExpiresIn > 0);
    }

    [Fact]
    public async Task Login_ShouldReturnUnauthorized_WhenPasswordIsInvalid()
    {
        var email = await RegisterUserAsync();

        var response = await _client.PostAsJsonAsync("/login", new { Email = email, Password = "Wr0ng-P4ssw0rd!42" });

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Login_ShouldReturnUnauthorized_WhenUserDoesNotExist()
    {
        var response = await _client.PostAsJsonAsync("/login", new { Email = "unknown.user@tasksync.test", Password });

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Login_ShouldIssueUsableTokens_WhenCredentialsAreValid()
    {
        var email = await RegisterUserAsync();

        var login = await _client.PostAsJsonAsync("/login", new { Email = email, Password });
        var loginTokens = await login.Content.ReadFromJsonAsync<TokenResponse>();
        Assert.NotNull(loginTokens);

        var refresh = await _client.PostAsJsonAsync("/refresh", new { loginTokens.RefreshToken });

        Assert.Equal(HttpStatusCode.OK, refresh.StatusCode);

        var refreshedTokens = await refresh.Content.ReadFromJsonAsync<TokenResponse>();
        Assert.NotNull(refreshedTokens);
        Assert.Equal("Bearer", refreshedTokens.TokenType);
        Assert.False(string.IsNullOrEmpty(refreshedTokens.AccessToken));
        Assert.False(string.IsNullOrEmpty(refreshedTokens.RefreshToken));
        Assert.NotEqual(loginTokens.AccessToken, refreshedTokens.AccessToken);
    }

    [Fact]
    public async Task Login_ShouldReturnUnauthorized_WhenAccountIsLockedOut()
    {
        // the identity API enables lockout on failed logins (default: 5 failed attempts lock the account for 5 minutes)
        var email = await RegisterUserAsync();

        // run the failed logins consecutively - a successful login would reset the failure counter
        for (var attempt = 0; attempt < 10; attempt++)
        {
            var failedLogin = await _client.PostAsJsonAsync("/login", new { Email = email, Password = "Wr0ng-P4ssw0rd!42" });
            Assert.Equal(HttpStatusCode.Unauthorized, failedLogin.StatusCode);
        }

        // the correct password must not bypass the lockout
        var lockedOutLogin = await _client.PostAsJsonAsync("/login", new { Email = email, Password });

        Assert.Equal(HttpStatusCode.Unauthorized, lockedOutLogin.StatusCode);

        var user = await FindUserByEmailAsync(email);
        Assert.NotNull(user);
        Assert.NotNull(user.LockoutEnd);
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // Helpers
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////

    private static string UniqueEmail()
    {
        return $"login.test+{Guid.NewGuid():N}@tasksync.test";
    }

    private async Task<string> RegisterUserAsync()
    {
        var email = UniqueEmail();

        var response = await _client.PostAsJsonAsync("/register", new { Email = email, Password });

        Assert.True(response.IsSuccessStatusCode, $"precondition failed: could not register user ({(int)response.StatusCode} {response.ReasonPhrase})");
        return email;
    }

    private async Task<UserEntity?> FindUserByEmailAsync(string email)
    {
        using var scope = _factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

        return await dbContext.Users.SingleOrDefaultAsync(u => u.Email!.ToLower() == email.ToLower());
    }

    private sealed record TokenResponse(string TokenType, string AccessToken, long ExpiresIn, string RefreshToken);
}
