using System.Net;

using Microsoft.AspNetCore.Identity;

using TaskSync.Infrastructure.Entities;

namespace TaskSync.Tests.IntegrationTests.User;

public class GetUserListTest : BaseIntegrationTest
{
    private const string Endpoint = "/api/user";

    public GetUserListTest(IntegrationTestWebAppFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task GetCurrentUser_ShouldReturn401_WhenNoAuthProvided()
    {
        await AssertEndpointsReturnUnauthorized([
            (Endpoint, HttpMethod.Get, null)
        ]);
    }

    [Fact]
    public async Task GetCurrentUser_ShouldReturn403_WhenUserIsNoAdmin()
    {
        SetAuthenticatedUser();

        var response = await _client.GetAsync(Endpoint);

        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }
    
    [Fact]
    public async Task GetCurrentUser_ShouldReturnList_WhenUserIsAdmin()
    {
        const string externalUserId = "idp|123";
        const string userEmail = "admin@tasksync.test";
        
        var adminUser = new UserEntity { ExternalUserId = externalUserId, Email = userEmail }; 
        
        // seed user-role relation
        await SeedAsync(adminUser);
        // Role ID "1" is the admin role predefined in database, see also data seed
        await SeedAsync(new IdentityUserRole<string> { RoleId = "1", UserId = adminUser.Id });
        
        SetAuthenticatedUser(null, externalUserId, userEmail);

        var response = await _client.GetAsync(Endpoint);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}
