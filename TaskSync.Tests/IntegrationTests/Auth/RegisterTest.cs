using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using TaskSync.Infrastructure;
using TaskSync.Infrastructure.Entities;

namespace TaskSync.Tests.IntegrationTests.Auth;

/// <summary>
/// Integration tests for the "POST /register" identity endpoint (mapped via <c>MapIdentityApi</c>).
/// </summary>
public class RegisterTest : BaseIntegrationTest
{
    private const string StrongPassword = "Str0ng-P4ssw0rd!42";

    public RegisterTest(IntegrationTestWebAppFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Register_ShouldCreateNewUser_WhenCredentialsAreValid()
    {
        var email = UniqueEmail();

        var response = await _client.PostAsJsonAsync("/register", new { Email = email, Password = StrongPassword });

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var user = await FindUserByEmailAsync(email);
        Assert.NotNull(user);
        Assert.Equal(email, user.Email);
        // the identity API uses the email address as user name
        Assert.Equal(email, user.UserName);
        Assert.False(user.EmailConfirmed);
        // the password must never be stored in plain text
        Assert.False(string.IsNullOrEmpty(user.PasswordHash));
    }

    [Fact]
    public async Task Register_ShouldReturnValidationProblem_WhenEmailIsMalformed()
    {
        var response = await _client.PostAsJsonAsync("/register", new { Email = "not-an-email-address", Password = StrongPassword });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Contains("InvalidEmail", await GetValidationErrorCodesAsync(response));
    }

    [Fact]
    public async Task Register_ShouldReturnValidationProblem_WhenPasswordIsTooShort()
    {
        var email = UniqueEmail();

        var response = await _client.PostAsJsonAsync("/register", new { Email = email, Password = "123" });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Contains("PasswordTooShort", await GetValidationErrorCodesAsync(response));

        // a rejected registration must not persist any user
        Assert.Null(await FindUserByEmailAsync(email));
    }

    [Fact]
    public async Task Register_ShouldReturnValidationProblem_WhenEmailIsAlreadyRegistered()
    {
        var email = UniqueEmail();

        var firstRegistration = await _client.PostAsJsonAsync("/register", new { Email = email, Password = StrongPassword });
        Assert.Equal(HttpStatusCode.OK, firstRegistration.StatusCode);

        var secondRegistration = await _client.PostAsJsonAsync("/register", new { Email = email, Password = StrongPassword });

        Assert.Equal(HttpStatusCode.BadRequest, secondRegistration.StatusCode);
        // since the identity API uses the email as user name, the duplicate is reported by the
        // username validator (unless email uniqueness is enforced separately)
        Assert.Contains(await GetValidationErrorCodesAsync(secondRegistration), code => code is "DuplicateUserName" or "DuplicateEmail");

        // the user must not have been registered a second time
        Assert.Equal(1, await CountUsersByEmailAsync(email));
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // Helpers
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////

    private static string UniqueEmail()
    {
        return $"register.test+{Guid.NewGuid():N}@tasksync.test";
    }

    private static async Task<string[]> GetValidationErrorCodesAsync(HttpResponseMessage response)
    {
        using var document = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        return [.. document.RootElement.GetProperty("errors").EnumerateObject().Select(property => property.Name)];
    }

    private async Task<UserEntity?> FindUserByEmailAsync(string email)
    {
        using var scope = _factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

        return await dbContext.Users.SingleOrDefaultAsync(u => u.Email!.ToLower() == email.ToLower());
    }

    private async Task<int> CountUsersByEmailAsync(string email)
    {
        using var scope = _factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

        return await dbContext.Users.CountAsync(u => u.Email!.ToLower() == email.ToLower());
    }
}
