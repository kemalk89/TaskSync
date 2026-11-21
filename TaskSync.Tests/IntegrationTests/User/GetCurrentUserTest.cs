using System.Net;
using System.Net.Http.Json;

using DomainUser = TaskSync.Domain.User.User;

namespace TaskSync.Tests.IntegrationTests.User;

public class GetCurrentUserTest : BaseIntegrationTest
{
    private const string CurrentUserEndpoint = "/api/user/current-user";

    public GetCurrentUserTest(IntegrationTestWebAppFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task GetCurrentUser_ShouldReturn401_WhenNoAuthProvided()
    {
        await AssertEndpointsReturnUnauthorized([
            (CurrentUserEndpoint, HttpMethod.Get, null)
        ]);
    }

    [Fact]
    public async Task GetCurrentUser_ShouldReturnCurrentUser_WhenAuthenticated()
    {
        SetAuthenticatedUser();

        var response = await _client.GetAsync(CurrentUserEndpoint);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var result = await response.Content.ReadFromJsonAsync<CurrentUserResponse>();
        Assert.NotNull(result);
        Assert.True(result.Success);
        Assert.NotNull(result.Value);
        Assert.Equal("6", result.Value.Id);
        Assert.Equal("integration_tests|01", result.Value.ExternalUserId);
        Assert.Equal("IntegrationTests.User1@tasksync.test", result.Value.Email);
        Assert.Equal("Test User1", result.Value.Username);
    }

    private sealed record CurrentUserResponse(bool Success, DomainUser? Value);
}
