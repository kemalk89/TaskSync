using System.Net;
using System.Net.Http.Json;

using TaskSync.Controllers.Project;

namespace TaskSync.Tests.IntegrationTests.Project.GetTeamMembers;

public class GetTeamMembersTest : BaseIntegrationTest
{
    public GetTeamMembersTest(IntegrationTestWebAppFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task GetTeamMembers_ShouldReturn200_WithSeededMembers()
    {
        SetAuthenticatedUser();

        var response = await _client.GetAsync("/api/project/1/team");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var members = await response.Content.ReadFromJsonAsync<List<ProjectMemberResponse>>();
        Assert.NotNull(members);
        Assert.Equal(3, members.Count);
        Assert.Contains(members, m => m.Role == "ProjectManager");
        Assert.All(members, m =>
        {
            Assert.NotNull(m.User);
            Assert.Equal(m.UserId, m.User.Id);
        });
    }

    [Fact]
    public async Task GetTeamMembers_ShouldReturn404_WhenProjectNotFound()
    {
        SetAuthenticatedUser();

        var response = await _client.GetAsync("/api/project/100000/team");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetTeamMembers_ShouldReturn401_WhenUnauthorized()
    {
        var response = await _client.GetAsync("/api/project/1/team");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
}
