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

    [Fact]
    public async Task Api_ShouldReturn401_WhenNoAuthProvided()
    {
        var endpoints = new (string Url, HttpMethod Method, object? Payload)[]
        {
            ("/api/project/123", HttpMethod.Get, null),
            ("/api/project", HttpMethod.Get, null),
            ("/api/project", HttpMethod.Post, new CreateProjectCommand())
        };
        
        foreach (var (url, method, payload) in endpoints)
        {
            HttpResponseMessage response;
            if (method == HttpMethod.Post)
            {
                response = await _client.PostAsJsonAsync(url, payload);
            } else if (method == HttpMethod.Get)
            {
                response = await _client.GetAsync(url);
                
            }
            else
            {
                throw new NotSupportedException($"Method {method} not supported");
            }

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
    
    [Fact]
    public async Task CreateTicket_ShouldReturn400_WhenInvalidRequestProvided()
    {
        SetAuthenticatedUser();

        var cmd = new CreateProjectCommand();
        
        var responseCreate = await _client.PostAsJsonAsync("/api/project", cmd);
        
        Assert.Equal(HttpStatusCode.BadRequest, responseCreate.StatusCode);
        
        var errors = await responseCreate.Content.ReadFromJsonAsync<ErrorResponse>();
        Assert.Equal( "'Title' must not be empty.", errors?.Errors[0]);
    }
}