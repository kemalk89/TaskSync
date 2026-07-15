using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

using TaskSync.Controllers.Project;
using TaskSync.Controllers.Request;
using TaskSync.Controllers.Response;
using TaskSync.Domain.Project.CreateProject;
using TaskSync.Domain.Shared;
using TaskSync.Domain.Ticket.CreateTicket;

namespace TaskSync.Tests.IntegrationTests.Project.AssignTicketToSprint;

public class AssignTicketToSprintTest : BaseIntegrationTest
{
    public AssignTicketToSprintTest(IntegrationTestWebAppFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task AssignTicketToSprint_ShouldReturn401_WhenUnauthorized()
    {
        await AssertEndpointsReturnUnauthorized([
            ("/api/project/1/sprint/0/ticket/1?newPosition=1", HttpMethod.Post, null)
        ]);
    }

    [Fact]
    public async Task AssignTicketToDraftSprint_ShouldFail_WhenProjectNotFound()
    {
        SetAuthenticatedUser();

        var response = await _client.PostAsync(
            "/api/project/999999/sprint/0/ticket/1?newPosition=1", null);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var document = await response.Content.ReadFromJsonAsync<JsonDocument>();
        Assert.NotNull(document);
        Assert.False(document.RootElement.GetProperty("success").GetBoolean());
        var errorList = GetErrorDetails(document);
        Assert.Contains("Project not found", errorList);
    }

    [Fact]
    public async Task AssignTicketToDraftSprint_ShouldCreateDraftAndAssign_WhenNoDraftExists()
    {
        SetAuthenticatedUser();

        // Create new project
        var projectId = await CreateNewProjectAsync();

        // Create new work item
        var ticketCmd = new CreateTicketCommand
        {
            Title = "Test Ticket For Draft",
            ProjectId = projectId
        };

        var createTicketResponse = await _client.PostAsJsonAsync("/api/ticket", ticketCmd);
        Assert.Equal(HttpStatusCode.Created, createTicketResponse.StatusCode);

        var ticketIdResponse = await createTicketResponse.Content.ReadFromJsonAsync<CreateTicketResponse>();
        var ticketId = ticketIdResponse?.TicketId;
        Assert.NotNull(ticketId);

        // Assign work item to draft sprint (sprintId = 0)
        var assignResponse = await _client.PostAsync(
            $"/api/project/{projectId}/sprint/0/ticket/{ticketId}?newPosition=1", null);

        Assert.Equal(HttpStatusCode.OK, assignResponse.StatusCode);

        var document = await assignResponse.Content.ReadFromJsonAsync<JsonDocument>();
        Assert.NotNull(document);
        Assert.True(document.RootElement.GetProperty("success").GetBoolean());
    }

    [Fact]
    public async Task AssignTicketToDraftSprint_ReusesExistingDraft_WhenAlreadyPresent()
    {
        SetAuthenticatedUser();

        // Step 1: Create new project
        var projectId = await CreateNewProjectAsync();

        // Step 2: Create ticket #1
        var ticket1Cmd = new CreateTicketCommand
        {
            Title = "First Ticket",
            ProjectId = projectId
        };

        var ticket1Response = await _client.PostAsJsonAsync("/api/ticket", ticket1Cmd);
        Assert.Equal(HttpStatusCode.Created, ticket1Response.StatusCode);

        var ticket1IdResponse = await ticket1Response.Content.ReadFromJsonAsync<CreateTicketResponse>();
        var ticket1Id = ticket1IdResponse?.TicketId;
        Assert.NotNull(ticket1Id);

        // Step 3: Assign ticket #1 to draft sprint
        var assign1Response = await _client.PostAsync(
            $"/api/project/{projectId}/sprint/0/ticket/{ticket1Id}?newPosition=1", null);
        Assert.Equal(HttpStatusCode.OK, assign1Response.StatusCode);

        //Step 4: Create ticket #2
        var ticket2Cmd = new CreateTicketCommand
        {
            Title = "Second Ticket",
            ProjectId = projectId
        };

        var ticket2Response = await _client.PostAsJsonAsync("/api/ticket", ticket2Cmd);
        Assert.Equal(HttpStatusCode.Created, ticket2Response.StatusCode);

        var ticket2IdResponse = await ticket2Response.Content.ReadFromJsonAsync<CreateTicketResponse>();
        var ticket2Id = ticket2IdResponse?.TicketId;
        Assert.NotNull(ticket2Id);

        // Step 5: Assign ticket #2 to draft sprint
        var assign2Response = await _client.PostAsync(
            $"/api/project/{projectId}/sprint/0/ticket/{ticket2Id}?newPosition=2", null);
        Assert.Equal(HttpStatusCode.OK, assign2Response.StatusCode);

        // Step 6: fetch draft sprint and verify both tickets are in the same draft
        var sprintsResponse = await _client.GetAsync($"/api/project/{projectId}/sprint/draft");
        Assert.Equal(HttpStatusCode.OK, sprintsResponse.StatusCode);

        var responseDoc = await sprintsResponse.Content.ReadFromJsonAsync<JsonDocument>();
        Assert.NotNull(responseDoc);
        Assert.True(responseDoc.RootElement.GetProperty("success").GetBoolean());

        responseDoc.RootElement.TryGetProperty("value", out var resultValue);
        resultValue.TryGetProperty("tickets", out var tickets);
        Assert.Equal(2, tickets.GetArrayLength());
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
