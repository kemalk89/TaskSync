using System.Net;
using System.Net.Http.Json;

using TaskSync.Controllers.Response;
using TaskSync.Domain.Project.Commands;

namespace TaskSync.Tests.IntegrationTests;

public class ProjectControllerTests : BaseIntegrationTest
{
    public ProjectControllerTests(IntegrationTestWebAppFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task CreateProject_ShouldReturn201_WhenValidRequest()
    {
        SetAuthenticatedUser();
        
        var cmd = new CreateProjectCommand
        {
            Title = "Test Project Title", Description = "Test Project Description"
        };
        
        // Test adding a new project
        var responseCreate = await _client.PostAsJsonAsync("/api/project", cmd);
        
        Assert.Equal(HttpStatusCode.Created, responseCreate.StatusCode);
        
        var createdProject = await responseCreate.Content.ReadFromJsonAsync<ProjectResponse>();
        Assert.Equal("Test Project Title", createdProject?.Title);

        // Test fetching the newly added project
        var responseGet = await _client.GetAsync($"/api/project/{createdProject?.Id}");
        
        Assert.Equal(HttpStatusCode.OK, responseGet.StatusCode);
        
        var fetchedProject = await responseGet.Content.ReadFromJsonAsync<ProjectResponse>();
        Assert.Equal("Test Project Title", fetchedProject?.Title);
    }
    
    /*
    [Fact]
    public async Task CreateTicket_ShouldReturn404_WhenProjectNotExists()
    {

    }
    */
}