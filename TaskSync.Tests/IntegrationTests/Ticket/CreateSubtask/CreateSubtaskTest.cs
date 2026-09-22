using System.Net;
using System.Net.Http.Json;

using TaskSync.Controllers.Request;
using TaskSync.Controllers.Response;
using TaskSync.Domain.Project.CreateProject;
using TaskSync.Domain.Shared;
using TaskSync.Domain.Ticket.CreateTicket;

namespace TaskSync.Tests.IntegrationTests.Ticket.CreateSubtask;

public class CreateSubtaskTest : BaseIntegrationTest, IClassFixture<CreateProjectFixture>
{
    private readonly CreateProjectFixture _createProjectFixture;

    public CreateSubtaskTest(
        IntegrationTestWebAppFactory factory,
        CreateProjectFixture createProjectFixture) : base(factory)
    {
        _createProjectFixture = createProjectFixture;
    }

    [Fact]
    public async Task CreateSubtask_ShouldReturn401_WhenNoAuthProvided()
    {
        await AssertEndpointsReturnUnauthorized([
            ("/api/ticket/1/subtask", HttpMethod.Post, new CreateTicketCommand())
        ]);
    }

    [Fact]
    public async Task CreateSubtask_ShouldReturn400_WhenInvalidRequestProvided()
    {
        SetAuthenticatedUser();

        var cmd = new CreateTicketCommand();

        var responseCreate = await _client.PostAsJsonAsync("/api/ticket/1/subtask", cmd);

        Assert.Equal(HttpStatusCode.BadRequest, responseCreate.StatusCode);

        var errors = await responseCreate.Content.ReadFromJsonAsync<ErrorResponse>();
        Assert.Equal(ResultCodes.ResultCodeValidationFailed, errors?.ErrorCode);
        Assert.Equal("'Title' must not be empty.", errors?.ErrorDetails[0]);
        Assert.Equal("'Project Id' must not be empty.", errors?.ErrorDetails[1]);
    }

    [Fact]
    public async Task CreateSubtask_ShouldReturn400_WhenParentIdIsNotPositive()
    {
        SetAuthenticatedUser();

        // ProjectId 1 is the seeded demo project.
        var cmd = new CreateTicketCommand
        {
            Title = "Test Subtask",
            ProjectId = 1
        };

        var responseCreate = await _client.PostAsJsonAsync("/api/ticket/0/subtask", cmd);

        Assert.Equal(HttpStatusCode.BadRequest, responseCreate.StatusCode);

        var errors = await responseCreate.Content.ReadFromJsonAsync<ErrorResponse>();
        Assert.Equal(ResultCodes.ResultCodeValidationFailed, errors?.ErrorCode);
        Assert.Contains("'Parent Id' must be greater than '0'.", errors?.ErrorDetails ?? []);
    }

    [Fact]
    public async Task CreateSubtask_ShouldReturn404_WhenNoProjectExists()
    {
        SetAuthenticatedUser();

        // Ticket 1 is a seeded ticket, but the project does not exist.
        var cmd = new CreateTicketCommand
        {
            Title = "Test Subtask",
            ProjectId = 123456789
        };

        var responseCreate = await _client.PostAsJsonAsync("/api/ticket/1/subtask", cmd);

        Assert.Equal(HttpStatusCode.NotFound, responseCreate.StatusCode);

        var errors = await responseCreate.Content.ReadFromJsonAsync<ErrorResponse>();
        Assert.Equal(ResultCodes.ResultCodeResourceNotFound, errors?.ErrorCode);
    }

    [Fact]
    public async Task CreateSubtask_ShouldReturn400_WhenParentTicketDoesNotExist()
    {
        SetAuthenticatedUser();

        // first, create a project
        var createdProject = await _createProjectFixture.InitIfNotExistsAsync(
            _client, new CreateProjectCommand { Title = "Test Project Title" });

        // next, try to create a subtask for a parent ticket that does not exist
        var cmd = new CreateTicketCommand
        {
            Title = "Test Subtask",
            ProjectId = createdProject.Id
        };

        var responseCreate = await _client.PostAsJsonAsync("/api/ticket/123456789/subtask", cmd);

        Assert.Equal(HttpStatusCode.BadRequest, responseCreate.StatusCode);

        var errors = await responseCreate.Content.ReadFromJsonAsync<ErrorResponse>();
        Assert.Equal(ResultCodes.ResultCodeValidationFailed, errors?.ErrorCode);
        Assert.Contains("Parent ticket with id 123456789 not exists", errors!.ErrorDetails);
    }

    [Fact]
    public async Task CreateSubtask_ShouldReturn400_WhenParentTicketIsSubtask()
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

        // next, create a subtask for the parent ticket
        var responseCreateSubtask = await _client.PostAsJsonAsync(
            $"/api/ticket/{createdParent.TicketId}/subtask",
            new CreateTicketCommand
            {
                Title = "Child Subtask",
                ProjectId = createdProject.Id
            });
        Assert.Equal(HttpStatusCode.Created, responseCreateSubtask.StatusCode);
        var createdSubtask = await responseCreateSubtask.Content.ReadFromJsonAsync<CreateTicketResponse>();
        Assert.NotNull(createdSubtask);

        // now, try to create a subtask for the subtask, which must be rejected
        var responseCreateSubSubtask = await _client.PostAsJsonAsync(
            $"/api/ticket/{createdSubtask.TicketId}/subtask",
            new CreateTicketCommand
            {
                Title = "Subtask of a subtask",
                ProjectId = createdProject.Id
            });

        Assert.Equal(HttpStatusCode.BadRequest, responseCreateSubSubtask.StatusCode);

        var errors = await responseCreateSubSubtask.Content.ReadFromJsonAsync<ErrorResponse>();
        Assert.Equal(ResultCodes.ResultCodeValidationFailed, errors?.ErrorCode);
        Assert.Contains(
            $"Parent ticket with id {createdSubtask.TicketId} is a subtask itself. A subtask cannot have a subtask.",
            errors?.ErrorDetails ?? []);
    }

    [Fact]
    public async Task CreateSubtask_ShouldReturn201_WhenValidRequest()
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

        // next, create a subtask for the parent ticket
        var createSubtaskCommand = new CreateTicketCommand
        {
            Title = "Child Subtask",
            Description = "A subtask of the parent ticket",
            ProjectId = createdProject.Id
        };

        var responseCreateSubtask = await _client.PostAsJsonAsync(
            $"/api/ticket/{createdParent.TicketId}/subtask", createSubtaskCommand);
        Assert.Equal(HttpStatusCode.Created, responseCreateSubtask.StatusCode);
        var createdSubtask = await responseCreateSubtask.Content.ReadFromJsonAsync<CreateTicketResponse>();
        Assert.NotNull(createdSubtask);
        Assert.NotEqual(createdParent.TicketId, createdSubtask.TicketId);

        // now, verify the subtask really references the parent ticket
        var subtask = await _client.GetFromJsonAsync<TicketResponse>(
            $"/api/ticket/{createdSubtask.TicketId}");
        Assert.NotNull(subtask);
        Assert.Equal("Child Subtask", subtask.Title);
        Assert.Equal(createdParent.TicketId, subtask.ParentId);
        Assert.NotNull(subtask.Project);
        Assert.Equal(createdProject.Id, subtask.Project.Id);

        // and verify the parent ticket is not itself a subtask
        var parent = await _client.GetFromJsonAsync<TicketResponse>(
            $"/api/ticket/{createdParent.TicketId}");
        Assert.NotNull(parent);
        Assert.Null(parent.ParentId);
    }

    [Fact]
    public async Task CreateSubtask_ShouldAllowMultipleSubtasksForOneParent()
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

        // next, create two subtasks for the same parent ticket
        var subtaskIds = new List<int>();
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
            var createdSubtask = await responseCreateSubtask.Content.ReadFromJsonAsync<CreateTicketResponse>();
            Assert.NotNull(createdSubtask);
            subtaskIds.Add(createdSubtask.TicketId);
        }

        // now, verify both subtasks reference the same parent ticket
        var subtask1 = await _client.GetFromJsonAsync<TicketResponse>($"/api/ticket/{subtaskIds[0]}");
        var subtask2 = await _client.GetFromJsonAsync<TicketResponse>($"/api/ticket/{subtaskIds[1]}");
        Assert.Equal(createdParent.TicketId, subtask1?.ParentId);
        Assert.Equal(createdParent.TicketId, subtask2?.ParentId);
    }
}
