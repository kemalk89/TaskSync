using System.Net;
using System.Net.Http.Json;

using TaskSync.Controllers.Project;
using TaskSync.Controllers.Request;
using TaskSync.Controllers.Response;
using TaskSync.Domain.Project.CreateProject;
using TaskSync.Domain.Shared;
using TaskSync.Domain.Sprint.AddSprint;
using TaskSync.Domain.Ticket.CreateTicket;

namespace TaskSync.Tests.IntegrationTests.Project.GetActiveSprint;

public class GetActiveSprintTest : BaseIntegrationTest
{
    public GetActiveSprintTest(IntegrationTestWebAppFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task GetActiveSprint_ShouldReturn401_WhenUnauthorized()
    {
        await AssertEndpointsReturnUnauthorized([
            ("/api/project/1/sprint/active", HttpMethod.Get, null)
        ]);
    }

    [Fact]
    public async Task GetActiveSprint_ShouldReturnNull_WhenNoSprintExists()
    {
        SetAuthenticatedUser();

        var projectId = await CreateNewProjectAsync();

        var response = await _client.GetAsync($"/api/project/{projectId}/sprint/active");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains(ResultCodes.ResultCodeResourceNotFound, content);
    }

    [Fact]
    public async Task GetActiveSprint_ShouldReturnNull_WhenSprintDatesAreInFuture()
    {
        SetAuthenticatedUser();

        var projectId = await CreateNewProjectAsync();

        var sprintCmd = new AddSprintCommand
        {
            StartDate = DateTimeOffset.UtcNow.AddDays(7),
            EndDate = DateTimeOffset.UtcNow.AddDays(14)
        };

        var createResponse = await _client.PostAsJsonAsync($"/api/project/{projectId}/sprint", sprintCmd);
        Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);

        var response = await _client.GetAsync($"/api/project/{projectId}/sprint/active");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains(ResultCodes.ResultCodeResourceNotFound, content);
    }

    [Fact]
    public async Task GetActiveSprint_ShouldReturnSprint_WhenCurrentlyActive()
    {
        SetAuthenticatedUser();

        var projectId = await CreateNewProjectAsync();

        var sprintCmd = new AddSprintCommand
        {
            StartDate = DateTimeOffset.UtcNow.AddDays(-1),
            EndDate = DateTimeOffset.UtcNow.AddDays(7)
        };

        var createResponse = await _client.PostAsJsonAsync($"/api/project/{projectId}/sprint", sprintCmd);
        Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);

        var idResponse = await createResponse.Content.ReadFromJsonAsync<IdResponse>();
        var sprintId = idResponse?.Id;
        Assert.NotNull(sprintId);

        var response = await _client.GetAsync($"/api/project/{projectId}/sprint/active");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var result = await response.Content.ReadFromJsonAsync<SprintResponse>();

        Assert.NotNull(result);
        Assert.Equal(sprintId, result.Id);
        Assert.NotNull(result.StartDate);
        Assert.NotNull(result.EndDate);
        Assert.NotNull(result.Tickets);
        Assert.Empty(result.Tickets);
    }

    [Fact]
    public async Task GetActiveSprint_ShouldReturnSprintWithTickets()
    {
        SetAuthenticatedUser();

        var projectId = await CreateNewProjectAsync();

        // Create a ticket
        var ticketCmd = new CreateTicketCommand
        {
            Title = "Test Ticket",
            ProjectId = projectId
        };

        var createTicketResponse = await _client.PostAsJsonAsync("/api/ticket", ticketCmd);
        Assert.Equal(HttpStatusCode.Created, createTicketResponse.StatusCode);

        var ticketIdResponse = await createTicketResponse.Content.ReadFromJsonAsync<CreateTicketResponse>();
        var ticketId = ticketIdResponse?.TicketId;
        Assert.NotNull(ticketId);

        // Create active sprint
        var sprintCmd = new AddSprintCommand
        {
            StartDate = DateTimeOffset.UtcNow.AddDays(-1),
            EndDate = DateTimeOffset.UtcNow.AddDays(7)
        };

        var createSprintResponse = await _client.PostAsJsonAsync($"/api/project/{projectId}/sprint", sprintCmd);
        Assert.Equal(HttpStatusCode.Created, createSprintResponse.StatusCode);

        var sprintIdResponse = await createSprintResponse.Content.ReadFromJsonAsync<IdResponse>();
        var sprintId = sprintIdResponse?.Id;
        Assert.NotNull(sprintId);

        // Assign ticket to sprint
        var assignResponse = await _client.PostAsync(
            $"/api/project/{projectId}/sprint/{sprintId}/ticket/{ticketId}?newPosition=1", null);
        Assert.Equal(HttpStatusCode.OK, assignResponse.StatusCode);

        var response = await _client.GetAsync($"/api/project/{projectId}/sprint/active");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var result = await response.Content.ReadFromJsonAsync<SprintResponse>();

        Assert.NotNull(result);
        Assert.Equal(sprintId, result.Id);
        Assert.NotNull(result.Tickets);
        Assert.Single(result.Tickets);
        Assert.Equal(ticketId, result.Tickets[0].Id);
        Assert.Equal("Test Ticket", result.Tickets[0].Title);
    }

    private async Task<int> CreateNewProjectAsync()
    {
        var cmd = new CreateProjectCommand
        {
            Title = "Test Project",
            Description = "Test Description"
        };

        var response = await _client.PostAsJsonAsync("/api/project", cmd);
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var createdProject = await response.Content.ReadFromJsonAsync<ProjectResponse>();
        Assert.NotNull(createdProject);

        return createdProject.Id;
    }
}
