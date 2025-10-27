using System.Net;
using System.Net.Http.Json;

using TaskSync.Controllers.Project;
using TaskSync.Domain.Project.CreateProject;

namespace TaskSync.Tests.IntegrationTests.Ticket;

public class CreateProjectFixture : IAsyncLifetime
{
    private ProjectResponse? ProjectResponse { get; set; }
    private HttpClient _httpClient = null!;
    
    public async Task<ProjectResponse> InitIfNotExistsAsync(
        HttpClient httpClient, CreateProjectCommand command)
    {
        _httpClient = httpClient;
        if (ProjectResponse == null)
        {
       
            var responseCreateProject = await _httpClient.PostAsJsonAsync("/api/project", command);
        
            Assert.Equal(HttpStatusCode.Created, responseCreateProject.StatusCode);
        
            var createdProject = await responseCreateProject.Content.ReadFromJsonAsync<ProjectResponse>();
            Assert.NotNull(createdProject);
            
            ProjectResponse = createdProject;
        }
        
        return ProjectResponse;
    }

    public Task InitializeAsync()
    {
        // Nothing to do here
        return Task.CompletedTask;
    }

    public async Task DisposeAsync()
    {
        if (ProjectResponse != null)
        {
            Console.WriteLine("TODO: Delete the project");
        }

        await Task.Delay(50);
    }
}