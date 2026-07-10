using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

using TaskSync.Controllers.Project;
using TaskSync.Domain.Project.CreateProject;
using TaskSync.Domain.Project.UpdateProject;

namespace TaskSync.Tests.IntegrationTests.Project.UpdateProject;

public class UpdateProjectTest : BaseIntegrationTest
{
    public UpdateProjectTest(IntegrationTestWebAppFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task UpdateProject_ShouldReturn401_WhenUnauthorized()
    {
        var cmd = new UpdateProjectCommand
        {
            Title = "Updated Title"
        };

        var response = await _client.PatchAsJsonAsync("/api/project/1", cmd);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task UpdateProject_ShouldReturnFailedResult_WhenProjectNotFound()
    {
        SetAuthenticatedUser();

        var cmd = new UpdateProjectCommand
        {
            Title = "Updated Title"
        };

        var response = await _client.PatchAsJsonAsync("/api/project/999999", cmd);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var document = await response.Content.ReadFromJsonAsync<JsonDocument>();
        Assert.NotNull(document);
        Assert.False(document.RootElement.GetProperty("success").GetBoolean());
        Assert.Equal("RESOURCE_NOT_FOUND", document.RootElement.GetProperty("error").GetString());
    }

    [Fact]
    public async Task UpdateProject_ShouldReturn200_WhenTitleUpdated()
    {
        SetAuthenticatedUser();

        var projectId = await CreateNewProjectAsync();

        var cmd = new UpdateProjectCommand
        {
            Title = "Updated Title"
        };

        var response = await _client.PatchAsJsonAsync($"/api/project/{projectId}", cmd);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var document = await response.Content.ReadFromJsonAsync<JsonDocument>();
        Assert.NotNull(document);
        Assert.True(document.RootElement.GetProperty("success").GetBoolean());
        Assert.True(document.RootElement.GetProperty("value").GetBoolean());

        // Verify the update persisted
        var getResponse = await _client.GetAsync($"/api/project/{projectId}");
        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);

        var project = await getResponse.Content.ReadFromJsonAsync<ProjectResponse>();
        Assert.NotNull(project);
        Assert.Equal("Updated Title", project.Title);
    }

    [Fact]
    public async Task UpdateProject_ShouldReturn200_WhenDescriptionUpdated()
    {
        SetAuthenticatedUser();

        var projectId = await CreateNewProjectAsync();

        var cmd = new UpdateProjectCommand
        {
            Description = "New description"
        };

        var response = await _client.PatchAsJsonAsync($"/api/project/{projectId}", cmd);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var document = await response.Content.ReadFromJsonAsync<JsonDocument>();
        Assert.NotNull(document);
        Assert.True(document.RootElement.GetProperty("success").GetBoolean());
        Assert.True(document.RootElement.GetProperty("value").GetBoolean());

        // Verify the update persisted
        var getResponse = await _client.GetAsync($"/api/project/{projectId}");
        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);

        var project = await getResponse.Content.ReadFromJsonAsync<ProjectResponse>();
        Assert.NotNull(project);
        Assert.Equal("New description", project.Description);
    }

    [Fact]
    public async Task UpdateProject_ShouldNotChangeFields_WhenNotProvided()
    {
        SetAuthenticatedUser();

        var originalTitle = "Original Title";
        var projectId = await CreateNewProjectAsync(originalTitle);

        // Update only description, leave title untouched
        var cmd = new UpdateProjectCommand
        {
            Description = "Updated description only"
        };

        var response = await _client.PatchAsJsonAsync($"/api/project/{projectId}", cmd);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        // Verify title is unchanged but description is updated
        var getResponse = await _client.GetAsync($"/api/project/{projectId}");
        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);

        var project = await getResponse.Content.ReadFromJsonAsync<ProjectResponse>();
        Assert.NotNull(project);
        Assert.Equal(originalTitle, project.Title);
        Assert.Equal("Updated description only", project.Description);
    }

    [Fact]
    public async Task UpdateProject_ShouldReturn200_WhenProjectManagerAssigned()
    {
        SetAuthenticatedUser();

        var projectId = await CreateNewProjectAsync();

        var cmd = new UpdateProjectCommand
        {
            ProjectManagerId = 1
        };

        var response = await _client.PatchAsJsonAsync($"/api/project/{projectId}", cmd);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var document = await response.Content.ReadFromJsonAsync<JsonDocument>();
        Assert.NotNull(document);
        Assert.True(document.RootElement.GetProperty("success").GetBoolean());
        Assert.True(document.RootElement.GetProperty("value").GetBoolean());
    }

    [Fact]
    public async Task UpdateProject_ShouldReturn200_WhenMultipleUpdatesApplied()
    {
        SetAuthenticatedUser();

        var projectId = await CreateNewProjectAsync();

        var cmd = new UpdateProjectCommand
        {
            Title = "New Title",
            Description = "New Description",
            ProjectManagerId = 1
        };

        var response = await _client.PatchAsJsonAsync($"/api/project/{projectId}", cmd);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var document = await response.Content.ReadFromJsonAsync<JsonDocument>();
        Assert.NotNull(document);
        Assert.True(document.RootElement.GetProperty("success").GetBoolean());
        Assert.True(document.RootElement.GetProperty("value").GetBoolean());

        // Verify all fields updated
        var getResponse = await _client.GetAsync($"/api/project/{projectId}");
        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);

        var project = await getResponse.Content.ReadFromJsonAsync<ProjectResponse>();
        Assert.NotNull(project);
        Assert.Equal("New Title", project.Title);
        Assert.Equal("New Description", project.Description);
        Assert.NotNull(project.ProjectMembers);
        Assert.Contains(project.ProjectMembers, m => m.Role == "ProjectManager" && m.UserId == 1);
    }

    private async Task<int> CreateNewProjectAsync(string? title = null)
    {
        var cmd = new CreateProjectCommand
        {
            Title = title ?? "Test Project",
            Description = "Test Description"
        };

        var response = await _client.PostAsJsonAsync("/api/project", cmd);
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var createdProject = await response.Content.ReadFromJsonAsync<ProjectResponse>();
        Assert.NotNull(createdProject);

        return createdProject.Id;
    }
}
