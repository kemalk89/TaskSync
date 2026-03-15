using System.Net;
using System.Net.Http.Json;

using TaskSync.Controllers.Request;

namespace TaskSync.Tests.IntegrationTests.User;

public class SyncExternalUserTest : BaseIntegrationTest
{
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
    public async Task SyncExternalUser_ShouldReturnCreated_WhenExternalUserExists()
    {
        // Arrange
        SetAuthenticatedUser();

        var request = new SyncExternalUserRequest
        {
            ExternalUserId = "external_123"
        };
        
        // Act
        var response = await _client.PostAsJsonAsync("/api/user/external", request);
        var content = await response.Content.ReadAsStringAsync();
        Console.WriteLine($"Status: {response.StatusCode}, Body: {content}");
        
        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }
}
