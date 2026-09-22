using System.Net;
using System.Net.Http.Json;

using TaskSync.Controllers.Request;
using TaskSync.Controllers.Response;
using TaskSync.Domain.Project.CreateProject;
using TaskSync.Domain.Ticket.CreateTicket;

namespace TaskSync.Tests.IntegrationTests.Ticket.QuerySubtask;

public class GetSubtasksTest : BaseIntegrationTest, IClassFixture<CreateProjectFixture>
{
    private readonly CreateProjectFixture _createProjectFixture;

    public GetSubtasksTest(
        IntegrationTestWebAppFactory factory,
        CreateProjectFixture createProjectFixture) : base(factory)
    {
        _createProjectFixture = createProjectFixture;
    }

    [Fact]
    public async Task GetSubtasks_ShouldReturn401_WhenNoAuthProvided()
    {
        await AssertEndpointsReturnUnauthorized([
            ("/api/ticket/1/subtask", HttpMethod.Get, null)
        ]);
    }

    [Fact]
    public async Task GetSubtasks_ShouldReturn404_WhenParentTicketDoesNotExist()
    {
        SetAuthenticatedUser();

        var response = await _client.GetAsync("/api/ticket/123456789/subtask");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetSubtasks_ShouldReturnEmptyList_WhenTicketHasNoSubtasks()
    {
        SetAuthenticatedUser();

        // first, create a project
        var createdProject = await _createProjectFixture.InitIfNotExistsAsync(
            _client, new CreateProjectCommand { Title = "Test Project Title" });

        // next, create a parent ticket
        var responseCreateParent = await _client.PostAsJsonAsync("/api/ticket",
            new CreateTicketCommand
            {
                Title = "Parent Ticket Without Subtasks",
                ProjectId = createdProject.Id
            });
        Assert.Equal(HttpStatusCode.Created, responseCreateParent.StatusCode);
        var createdParent = await responseCreateParent.Content.ReadFromJsonAsync<CreateTicketResponse>();
        Assert.NotNull(createdParent);

        // now, load the subtasks of the parent ticket
        var subtasks = await _client.GetFromJsonAsync<List<TicketResponse>>(
            $"/api/ticket/{createdParent.TicketId}/subtask");

        Assert.NotNull(subtasks);
        Assert.Empty(subtasks);
    }

    [Fact]
    public async Task GetSubtasks_ShouldReturnSubtasksOfParentTicket()
    {
        SetAuthenticatedUser();

        // first, create a project
        var createdProject = await _createProjectFixture.InitIfNotExistsAsync(
            _client, new CreateProjectCommand { Title = "Test Project Title" });

        // next, create a parent ticket
        var responseCreateParent = await _client.PostAsJsonAsync("/api/ticket",
            new CreateTicketCommand
            {
                Title = "Parent Ticket",
                ProjectId = createdProject.Id
            });
        Assert.Equal(HttpStatusCode.Created, responseCreateParent.StatusCode);
        var createdParent = await responseCreateParent.Content.ReadFromJsonAsync<CreateTicketResponse>();
        Assert.NotNull(createdParent);

        // next, create two subtasks for the parent ticket
        for (var i = 1; i <= 2; i++)
        {
            var responseCreateSubtask = await _client.PostAsJsonAsync(
                $"/api/ticket/{createdParent.TicketId}/subtask",
                new CreateTicketCommand
                {
                    Title = $"Subtask {i}",
                    ProjectId = createdProject.Id
                });
            Assert.Equal(HttpStatusCode.Created, responseCreateSubtask.StatusCode);
        }

        // now, load the subtasks of the parent ticket
        var subtasks = await _client.GetFromJsonAsync<List<TicketResponse>>(
            $"/api/ticket/{createdParent.TicketId}/subtask");

        Assert.NotNull(subtasks);
        Assert.Equal(2, subtasks.Count);
        Assert.All(subtasks, subtask => Assert.Equal(createdParent.TicketId, subtask.ParentId));
        Assert.Contains(subtasks, s => s.Title == "Subtask 1");
        Assert.Contains(subtasks, s => s.Title == "Subtask 2");
    }

    [Fact]
    public async Task GetSubtasks_ShouldNotReturnSubtasksOfOtherTickets()
    {
        SetAuthenticatedUser();

        // first, create a project
        var createdProject = await _createProjectFixture.InitIfNotExistsAsync(
            _client, new CreateProjectCommand { Title = "Test Project Title" });

        // next, create two parent tickets
        var parentIds = new List<int>();
        for (var i = 1; i <= 2; i++)
        {
            var responseCreateParent = await _client.PostAsJsonAsync("/api/ticket",
                new CreateTicketCommand
                {
                    Title = $"Parent Ticket {i}",
                    ProjectId = createdProject.Id
                });
            Assert.Equal(HttpStatusCode.Created, responseCreateParent.StatusCode);
            var createdParent = await responseCreateParent.Content.ReadFromJsonAsync<CreateTicketResponse>();
            Assert.NotNull(createdParent);
            parentIds.Add(createdParent.TicketId);
        }

        // next, create a subtask for the first parent ticket only
        var responseCreateSubtask = await _client.PostAsJsonAsync(
            $"/api/ticket/{parentIds[0]}/subtask",
            new CreateTicketCommand
            {
                Title = "Subtask of first parent",
                ProjectId = createdProject.Id
            });
        Assert.Equal(HttpStatusCode.Created, responseCreateSubtask.StatusCode);

        // now, verify the second parent ticket has no subtasks
        var subtasksOfSecondParent = await _client.GetFromJsonAsync<List<TicketResponse>>(
            $"/api/ticket/{parentIds[1]}/subtask");

        Assert.NotNull(subtasksOfSecondParent);
        Assert.Empty(subtasksOfSecondParent);

        // and the first parent ticket has exactly its own subtask
        var subtasksOfFirstParent = await _client.GetFromJsonAsync<List<TicketResponse>>(
            $"/api/ticket/{parentIds[0]}/subtask");

        Assert.NotNull(subtasksOfFirstParent);
        Assert.Single(subtasksOfFirstParent);
        Assert.Equal("Subtask of first parent", subtasksOfFirstParent[0].Title);
    }
}
