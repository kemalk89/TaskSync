using System.Net;
using System.Text.Json;

using TaskSync.Controllers.Project;
using TaskSync.Domain.Shared;

namespace TaskSync.Tests.IntegrationTests.Project.QueryProject;

public class QueryProjectTest : BaseIntegrationTest
{
    public QueryProjectTest(IntegrationTestWebAppFactory factory) : base(factory)
    {
    }
    
    [Fact]
    public async Task GetProjects_ShouldReturn200()
    {
        SetAuthenticatedUser();
        
        var responseGet = await _client.GetAsync("/api/project");
        
        Assert.Equal(HttpStatusCode.OK, responseGet.StatusCode);
        
        var json = await responseGet.Content.ReadAsStringAsync();
        var list = JsonSerializer.Deserialize<PagedResult<ProjectResponse>>(json);
        Assert.NotNull(list);
    }
    
    [Fact]
    public async Task GetProjectById_ShouldReturn200()
    {
        SetAuthenticatedUser();
        
        var responseGet = await _client.GetAsync("/api/project/1");
        
        Assert.Equal(HttpStatusCode.OK, responseGet.StatusCode);
        
        var json = await responseGet.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<ProjectResponse>(json);
        Assert.NotNull(result);
    }
    
    [Fact]
    public async Task GetProjectById_ShouldReturn404()
    {
        SetAuthenticatedUser();
        
        var responseGet = await _client.GetAsync("/api/project/100000");
        
        Assert.Equal(HttpStatusCode.NotFound, responseGet.StatusCode);
    }
}