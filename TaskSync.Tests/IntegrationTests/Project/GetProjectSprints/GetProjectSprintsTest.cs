using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

using TaskSync.Controllers.Project;
using TaskSync.Controllers.Response;
using TaskSync.Domain.Project.CreateProject;
using TaskSync.Domain.Shared;
using TaskSync.Domain.Sprint.AddSprint;

namespace TaskSync.Tests.IntegrationTests.Project.GetProjectSprints;

public class GetProjectSprintsTest : BaseIntegrationTest
{
    public GetProjectSprintsTest(IntegrationTestWebAppFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task GetProjectSprints_ShouldReturn401_WhenUnauthorized()
    {
        await AssertEndpointsReturnUnauthorized([
            ("/api/project/1/sprints", HttpMethod.Get, null)
        ]);
    }

    [Fact]
    public async Task GetProjectSprints_ShouldReturn400_WhenInvalidPagination()
    {
        SetAuthenticatedUser();

        var response = await _client.GetAsync("/api/project/1/sprints?pageNumber=0");

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var document = await response.Content.ReadFromJsonAsync<JsonDocument>();
        var errorList = GetErrorDetails(document);
        Assert.Contains("'Page Number' must be greater than '0'.", errorList);
    }

    [Fact]
    public async Task GetProjectSprints_ShouldReturn400_WhenPageSizeExceedsMaximum()
    {
        SetAuthenticatedUser();

        var response = await _client.GetAsync("/api/project/1/sprints?pageSize=101");

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var document = await response.Content.ReadFromJsonAsync<JsonDocument>();
        var errorList = GetErrorDetails(document);
        Assert.Contains("'Page Size' must be less than or equal to '100'.", errorList);
    }

    [Fact]
    public async Task GetProjectSprints_ShouldReturn200_WithEmptyResult_WhenNoSprints()
    {
        SetAuthenticatedUser();

        var projectId = await CreateNewProjectAsync();

        var response = await _client.GetAsync($"/api/project/{projectId}/sprints");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var result = await response.Content.ReadFromJsonAsync<PagedResult<SprintResponse>>();

        Assert.NotNull(result);
        Assert.Equal(1, result.PageNumber);
        Assert.Equal(50, result.PageSize);
        Assert.Equal(0, result.Total);
        Assert.Empty(result.Items);
    }

    [Fact]
    public async Task GetProjectSprints_ShouldReturn200_WithSprints()
    {
        SetAuthenticatedUser();

        var projectId = await CreateNewProjectAsync();

        // Create a sprint
        var sprintCmd = new AddSprintCommand
        {
            EndDate = DateTime.Today.AddDays(3 * 7).ToUniversalTime()
        };

        var startSprintResponse = await _client.PostAsJsonAsync($"/api/project/{projectId}/sprint", sprintCmd);
        Assert.Equal(HttpStatusCode.Created, startSprintResponse.StatusCode);

        // Query sprints
        var response = await _client.GetAsync($"/api/project/{projectId}/sprints");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var result = await response.Content.ReadFromJsonAsync<PagedResult<SprintResponse>>();

        Assert.NotNull(result);
        Assert.Equal(1, result.PageNumber);
        Assert.Equal(50, result.PageSize);
        Assert.Equal(1, result.Total);
        Assert.Single(result.Items);

        var sprint = result.Items.First();
        Assert.True(sprint.Id > 0);
        Assert.NotNull(sprint.StartDate);
        Assert.NotNull(sprint.EndDate);
    }

    private async Task<int?> CreateNewProjectAsync()
    {
        var newProjectCmd = new CreateProjectCommand
        {
            Title = "Test Project Title",
            Description = "Test Project Description"
        };
        var responseCreateProject = await _client.PostAsJsonAsync("/api/project", newProjectCmd);

        Assert.Equal(HttpStatusCode.Created, responseCreateProject.StatusCode);

        var createdProject = await responseCreateProject.Content.ReadFromJsonAsync<ProjectResponse>();
        var projectId = createdProject?.Id;
        Assert.NotNull(projectId);

        return projectId;
    }
}
