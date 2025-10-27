using System.Net;
using System.Net.Http.Json;

using TaskSync.Controllers.Project;
using TaskSync.Controllers.Response;
using TaskSync.Domain.Project.CreateProject;
using TaskSync.Domain.Shared;

namespace TaskSync.Tests.IntegrationTests.Project.CreateProject;

public class CreateProjectTest : BaseIntegrationTest
{
    public CreateProjectTest(IntegrationTestWebAppFactory factory) : base(factory)
    {
    }
    
    [Fact]
    public async Task CreateTicket_ShouldReturn401_WhenNoAuthProvided()
    {
        await AssertEndpointsReturnUnauthorized([
            ("/api/project", HttpMethod.Post, new CreateProjectCommand())
        ]);
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


    
    [Fact]
    public async Task CreateTicket_ShouldReturn400_WhenInvalidRequestProvided()
    {
        SetAuthenticatedUser();

        var cmd = new CreateProjectCommand();
        
        var responseCreate = await _client.PostAsJsonAsync("/api/project", cmd);
        
        Assert.Equal(HttpStatusCode.BadRequest, responseCreate.StatusCode);
        
        var errors = await responseCreate.Content.ReadFromJsonAsync<ErrorResponse>();
        Assert.Equal(ResultCodes.ResultCodeValidationFailed, errors?.ErrorCode);
        Assert.Equal( "'Title' must not be empty.", errors?.ErrorDetails[0]);
    }
}
