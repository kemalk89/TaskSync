using System.Net;
using System.Net.Http.Json;
using System.Security.Claims;

using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using TaskSync.Controllers.Request;
using TaskSync.Infrastructure;
using TaskSync.Infrastructure.Entities;

namespace TaskSync.Tests.IntegrationTests.User;

public class SyncExternalUserTest : BaseIntegrationTest
{
    private const string MockEmail = "Test.Tester@tasksync.test";

    public SyncExternalUserTest(IntegrationTestWebAppFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task SyncExternalUser_ShouldReturn401_WhenNoAuthProvided()
    {
        await AssertEndpointsReturnUnauthorized([
            ("/api/user/external", HttpMethod.Post, new SyncExternalUserRequest())
        ]);
    }

    [Fact]
    public async Task SyncExternalUser_ShouldReturnNotFound_WhenExternalUserDoesNotExist()
    {
        SetAuthenticatedUser();

        var request = new SyncExternalUserRequest
        {
            ExternalUserId = "non_existent_external_id"
        };

        var response = await _client.PostAsJsonAsync("/api/user/external", request);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task SyncExternalUser_ShouldReturnCreated_WhenUserDoesNotExistLocally()
    {
        SetAuthenticatedUser();

        // Ensure no local user exists for the mock's email
        await DeleteUserByEmailAsync(MockEmail);

        var request = new SyncExternalUserRequest
        {
            ExternalUserId = "external_123"
        };

        var response = await _client.PostAsJsonAsync("/api/user/external", request);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        // Assert the user was persisted in the database
        var dbUser = await GetUserByEmailAsync(MockEmail);
        Assert.NotNull(dbUser);
        Assert.Equal(MockEmail, dbUser.Email);
        Assert.Equal("testuser", dbUser.Username);
        Assert.Equal("external_123", dbUser.ExternalUserId);
    }

    [Fact]
    public async Task SyncExternalUser_ShouldReturnOk_WhenUserAlreadyExists()
    {
        SetAuthenticatedUser();

        await DeleteUserByEmailAsync(MockEmail);
        await InsertUserAsync(MockEmail, "existing_user", null, "external_456");

        var request = new SyncExternalUserRequest
        {
            ExternalUserId = "external_456"
        };

        var response = await _client.PostAsJsonAsync("/api/user/external", request);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        // User should still exist (not duplicated)
        var users = await GetAllUsersByEmailAsync(MockEmail);
        Assert.Single(users);
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // Helpers
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////

    private IServiceScope CreateScopedServicesWithFakeAuth()
    {
        var scope = _factory.Services.CreateScope();
        var httpContextAccessor = scope.ServiceProvider.GetRequiredService<IHttpContextAccessor>();

        var httpContext = new DefaultHttpContext
        {
            User = new ClaimsPrincipal(new ClaimsIdentity(
            [
                new Claim(ClaimTypes.Name, "integration_tests|01")
            ], "TestScheme"))
        };
        httpContextAccessor.HttpContext = httpContext;

        return scope;
    }

    private async Task<Domain.User.User?> GetUserByEmailAsync(string email)
    {
        using var scope = CreateScopedServicesWithFakeAuth();
        var dbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

        var entity = await dbContext.Users
            .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());

        if (entity == null) return null;

        return new Domain.User.User
        {
            Id = entity.Id,
            Email = entity.Email,
            Username = entity.Username,
            Picture = entity.Picture,
            ExternalUserId = entity.ExternalUserId,
            SelectedLanguage = entity.SelectedLanguage,
            CreatedDate = entity.CreatedDate,
            ModifiedDate = entity.ModifiedDate
        };
    }

    private async Task<List<Domain.User.User>> GetAllUsersByEmailAsync(string email)
    {
        using var scope = CreateScopedServicesWithFakeAuth();
        var dbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

        var entities = await dbContext.Users
            .Where(u => u.Email.ToLower() == email.ToLower())
            .ToListAsync();

        return [.. entities.Select(e => new Domain.User.User
        {
            Id = e.Id,
            Email = e.Email,
            Username = e.Username,
            Picture = e.Picture,
            ExternalUserId = e.ExternalUserId,
            SelectedLanguage = e.SelectedLanguage,
            CreatedDate = e.CreatedDate,
            ModifiedDate = e.ModifiedDate
        })];
    }

    private async Task InsertUserAsync(string email, string? username, string? picture, string? externalUserId)
    {
        using var scope = CreateScopedServicesWithFakeAuth();
        var dbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

        var entity = new UserEntity
        {
            Email = email,
            Username = username,
            Picture = picture,
            ExternalUserId = externalUserId,
            CreatedDate = DateTimeOffset.Now
        };

        await dbContext.Users.AddAsync(entity);
        await dbContext.SaveChangesAsync();
    }

    private async Task DeleteUserByEmailAsync(string email)
    {
        using var scope = CreateScopedServicesWithFakeAuth();
        var dbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

        var entity = await dbContext.Users
            .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());

        if (entity != null)
        {
            dbContext.Users.Remove(entity);
            await dbContext.SaveChangesAsync();
        }
    }
}
