using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

using TaskSync.Controllers.Project;

using TaskSync.Controllers.Response;
using TaskSync.Domain.Project.CreateProject;

using TaskSync.Domain.Sprint.AddSprint;

namespace TaskSync.Tests.IntegrationTests.Project.CreateProject;

public class CreateSprintTest : BaseIntegrationTest
{
    public CreateSprintTest(IntegrationTestWebAppFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task CreateSprint_ShouldReturn401_WhenUnauthorized()
    {
        await AssertEndpointsReturnUnauthorized([
            ("/api/project/1/sprint", HttpMethod.Post, new AddSprintCommand())
        ]);
    }

    [Fact]
    public async Task CreateSprint_ShouldReturn400_WhenEndDateIsInPast()
    {
        SetAuthenticatedUser();

        var projectId = 0;
        var cmd = new AddSprintCommand
        {
            EndDate = DateTime.Today.AddDays(-3 * 7).ToUniversalTime()
        };

        var responseCreate = await _client.PostAsJsonAsync($"/api/project/{projectId}/sprint", cmd);
        var document = await responseCreate.Content.ReadFromJsonAsync<JsonDocument>();

        Assert.Equal(HttpStatusCode.BadRequest, responseCreate.StatusCode);

        var errorList = GetErrorDetails(document);
        Assert.Contains("The Sprint EndDate cannot be in the past", errorList);
    }

    [Fact]
    public async Task CreateSprint_ShouldReturn400_WhenProjectIdIsNull()
    {
        SetAuthenticatedUser();

        var projectId = 0;
        var cmd = new AddSprintCommand
        {
            EndDate = DateTime.Today.AddDays(3 * 7).ToUniversalTime()
        };

        var responseCreate = await _client.PostAsJsonAsync($"/api/project/{projectId}/sprint", cmd);
        var document = await responseCreate.Content.ReadFromJsonAsync<JsonDocument>();

        Assert.Equal(HttpStatusCode.BadRequest, responseCreate.StatusCode);

        var errorList = GetErrorDetails(document);
        Assert.Contains("'Project Id' must be greater than '0'.", errorList);
    }

    [Fact]
    public async Task CreateSprint_ShouldReturn400_WhenProjectNotExists()
    {
        SetAuthenticatedUser();

        var projectId = int.MaxValue;
        var cmd = new AddSprintCommand
        {
            EndDate = DateTime.Today.AddDays(3 * 7).ToUniversalTime()
        };

        var responseCreate = await _client.PostAsJsonAsync($"/api/project/{projectId}/sprint", cmd);
        var document = await responseCreate.Content.ReadFromJsonAsync<JsonDocument>();

        Assert.Equal(HttpStatusCode.BadRequest, responseCreate.StatusCode);

        var errorList = GetErrorDetails(document);
        Assert.Contains("Project with id 2147483647 not exists", errorList);
    }

    [Fact]
    public async Task CreateSprint_ShouldReturn400_WhenActiveSprintExists()
    {
        SetAuthenticatedUser();

        // first create a new project
        var projectId = await CreateNewProjectAsync();

        // next, start first sprint
        var newSprintCmd = new AddSprintCommand
        {
            EndDate = DateTime.Today.AddDays(3 * 7).ToUniversalTime()
        };

        var responseStartSprint = await _client.PostAsJsonAsync($"/api/project/{projectId}/sprint", newSprintCmd);
        Assert.Equal(HttpStatusCode.Created, responseStartSprint.StatusCode);

        // Finally, try starting a new sprint -> should fail because a sprint already running
        var responseStartSecondSprint = await _client.PostAsJsonAsync($"/api/project/{projectId}/sprint", newSprintCmd);
        Assert.Equal(HttpStatusCode.BadRequest, responseStartSecondSprint.StatusCode);

        var document = await responseStartSecondSprint.Content.ReadFromJsonAsync<JsonDocument>();

        var errorList = GetErrorDetails(document);
        Assert.Contains($"A sprint is already running for this project {projectId}", errorList);
    }

    [Fact]
    public async Task CreateSprint_ShouldReturn201_WhenValidRequest()
    {
        SetAuthenticatedUser();

        var projectId = await CreateNewProjectAsync();

        var cmd = new AddSprintCommand
        {
            EndDate = DateTime.Today.AddDays(3 * 7).ToUniversalTime()
        };

        var responseCreate = await _client.PostAsJsonAsync($"/api/project/{projectId}/sprint", cmd);

        Assert.Equal(HttpStatusCode.Created, responseCreate.StatusCode);

        var idResponse = await responseCreate.Content.ReadFromJsonAsync<IdResponse>();
        Assert.True(idResponse?.Id > 0);
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