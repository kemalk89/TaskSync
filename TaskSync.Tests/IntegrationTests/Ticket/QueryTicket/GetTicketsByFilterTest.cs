using System.Net;
using System.Net.Http.Json;

using TaskSync.Controllers.Response;
using TaskSync.Controllers.Project;
using TaskSync.Domain.Project.CreateProject;
using TaskSync.Domain.Shared;
using TaskSync.Domain.Ticket.CreateTicket;

namespace TaskSync.Tests.IntegrationTests.Ticket.QueryTicket;

public class GetTicketsByFilterTest : BaseIntegrationTest, IClassFixture<CreateProjectFixture>
{
    private readonly CreateProjectFixture _createProjectFixture;
    
    public GetTicketsByFilterTest(
        IntegrationTestWebAppFactory factory, CreateProjectFixture createProjectFixture) : base(factory)
    {
        _createProjectFixture = createProjectFixture;
    }
    
    [Fact]
    public async Task FilterTicketsByStatusIds()
    {
        SetAuthenticatedUser();
        
        // Step 1: Create a project
        var createdProject = await _createProjectFixture.InitIfNotExistsAsync(
            _client, new CreateProjectCommand { Title = "Test Project Title" });
        Assert.NotNull(createdProject);
        
        // Step 2: Create 2 tickets with different status
        var createTicketCommand_1 = new CreateTicketCommand
        {
            Title = "Ticket in TODO",
            ProjectId = createdProject.Id,
            StatusId = 1
        };
        
        var responseCreateTicket_1 = await _client.PostAsJsonAsync("/api/ticket", createTicketCommand_1);
        Assert.Equal(HttpStatusCode.Created, responseCreateTicket_1.StatusCode);
        
        var createTicketCommand_2 = new CreateTicketCommand
        {
            Title = "Ticket in Progress",
            ProjectId = createdProject.Id,
            StatusId = 2
        };
        
        var responseCreateTicket_2 = await _client.PostAsJsonAsync("/api/ticket", createTicketCommand_2);
        Assert.Equal(HttpStatusCode.Created, responseCreateTicket_2.StatusCode);
        
        // Step 3: Run filters
        var onlyTicketsInTodo = 
            await _client.GetFromJsonAsync<PagedResult<TicketResponse>>($"/api/ticket?status=1");
        
        var onlyTicketsInProgress = 
            await _client.GetFromJsonAsync<PagedResult<TicketResponse>>($"/api/ticket?status=2");
        
        var onlyTicketsInTodoAndInProgress = 
            await _client.GetFromJsonAsync<PagedResult<TicketResponse>>($"/api/ticket?status=1,2");
        
        // Assert
        Assert.NotNull(onlyTicketsInTodo);
        Assert.Contains(onlyTicketsInTodo.Items, response => "Ticket in TODO".Equals(response.Title));
        Assert.DoesNotContain(onlyTicketsInTodo.Items, response => "Ticket in Progress".Equals(response.Title));
        
        Assert.NotNull(onlyTicketsInProgress);
        Assert.Contains(onlyTicketsInProgress.Items, response => "Ticket in Progress".Equals(response.Title));
        Assert.DoesNotContain(onlyTicketsInProgress.Items, response => "Ticket in TODO".Equals(response.Title));
        
        Assert.NotNull(onlyTicketsInTodoAndInProgress);
        Assert.Contains(onlyTicketsInTodoAndInProgress.Items, response => "Ticket in Progress".Equals(response.Title));
        Assert.Contains(onlyTicketsInTodoAndInProgress.Items, response => "Ticket in TODO".Equals(response.Title));
    }
    
    [Fact]
    public async Task FilterTicketsByProjectIds()
    {
        SetAuthenticatedUser();
        
        // Step 1: Create two projects
        var createdProject_1 = await _createProjectFixture.InitIfNotExistsAsync(
            _client, new CreateProjectCommand { Title = "Test Project Title" });
        Assert.NotNull(createdProject_1);
        
        var responseCreateProject_2 = await _client.PostAsJsonAsync("/api/project", new CreateProjectCommand { Title = "Test Project Title 2" });
        Assert.Equal(HttpStatusCode.Created, responseCreateProject_2.StatusCode);
        
        var createdProject_2 = await responseCreateProject_2.Content.ReadFromJsonAsync<ProjectResponse>();
        Assert.NotNull(createdProject_2);
        
        // Step 2: Assign tickets to both projects
        var createTicketCommand_1 = new CreateTicketCommand
        {
            Title = "Ticket for project 1",
            ProjectId = createdProject_1.Id,
        };
        
        var responseCreateTicket_1 = await _client.PostAsJsonAsync("/api/ticket", createTicketCommand_1);
        Assert.Equal(HttpStatusCode.Created, responseCreateTicket_1.StatusCode);
        
        var createTicketCommand_2 = new CreateTicketCommand
        {
            Title = "Ticket for project 2",
            ProjectId = createdProject_2.Id
        };
        
        var responseCreateTicket_2 = await _client.PostAsJsonAsync("/api/ticket", createTicketCommand_2);
        Assert.Equal(HttpStatusCode.Created, responseCreateTicket_2.StatusCode);
        
        // Step 3: Run filters
        var onlyTicketInProject1 = 
            await _client.GetFromJsonAsync<PagedResult<TicketResponse>>($"/api/ticket?projects={createdProject_1.Id}");
        
        var onlyTicketInProject2 = 
            await _client.GetFromJsonAsync<PagedResult<TicketResponse>>($"/api/ticket?projects={createdProject_2.Id}");
        
        var onlyTicketsInBothProjects = 
            await _client.GetFromJsonAsync<PagedResult<TicketResponse>>($"/api/ticket?projects={createdProject_1.Id},{createdProject_2.Id}");
        
        // Assert
        Assert.NotNull(onlyTicketInProject1);
        Assert.Contains(onlyTicketInProject1.Items, response => "Ticket for project 1".Equals(response.Title));
        Assert.DoesNotContain(onlyTicketInProject1.Items, response => "Ticket for project 2".Equals(response.Title));
        
        Assert.NotNull(onlyTicketInProject2);
        Assert.Contains(onlyTicketInProject2.Items, response => "Ticket for project 2".Equals(response.Title));
        Assert.DoesNotContain(onlyTicketInProject2.Items, response => "Ticket for project 1".Equals(response.Title));
        
        Assert.NotNull(onlyTicketsInBothProjects);
        Assert.Contains(onlyTicketsInBothProjects.Items, response => "Ticket for project 1".Equals(response.Title));
        Assert.Contains(onlyTicketsInBothProjects.Items, response => "Ticket for project 2".Equals(response.Title));
        
        // Cleanup only test project 2 -> The first one will be automatically cleaned up by fixture
        var responseDeleteProject = await _client.DeleteAsync($"/api/project/{createdProject_2.Id}");
        Assert.Equal(HttpStatusCode.NoContent, responseDeleteProject.StatusCode);
    }

     [Fact]
    public async Task FilterTicketsByAssigneeIds()
    {
        SetAuthenticatedUser();
        
        // Step 1: Create projects
        var createdProject = await _createProjectFixture.InitIfNotExistsAsync(
            _client, new CreateProjectCommand { Title = "Test Project Title" });
        Assert.NotNull(createdProject);
        
        // Step 2: Assign ticket having assignee to the project
        var createTicketCommand = new CreateTicketCommand
        {
            Title = "Ticket for project 1",
            ProjectId = createdProject.Id,
            Assignee = 1
        };
        
        var responseCreateTicket = await _client.PostAsJsonAsync("/api/ticket", createTicketCommand);
        Assert.Equal(HttpStatusCode.Created, responseCreateTicket.StatusCode);
        
        // Step 3: Run filters
        var ticketsOfAssignee = 
            await _client.GetFromJsonAsync<PagedResult<TicketResponse>>($"/api/ticket?assignees=1");
        
        var ticketsOfOtherAssignee = 
            await _client.GetFromJsonAsync<PagedResult<TicketResponse>>($"/api/ticket?assignees=2");
        
        // Step 4: Assert
        Assert.NotNull(ticketsOfAssignee);
        Assert.Contains(ticketsOfAssignee.Items, response => "Ticket for project 1".Equals(response.Title));
        
        Assert.NotNull(ticketsOfOtherAssignee);
        Assert.DoesNotContain(ticketsOfOtherAssignee.Items, response => "Ticket for project 1".Equals(response.Title));
    }
    
    [Fact]
    public async Task FilterTicketsByProjectIdsAndAssigneeIds()
    {
        SetAuthenticatedUser();

        // Step 1: Create two projects
        var createdProject1 = await _createProjectFixture.InitIfNotExistsAsync(
            _client, new CreateProjectCommand { Title = "Project A" });
        Assert.NotNull(createdProject1);

        var responseCreateProject2 = await _client.PostAsJsonAsync("/api/project", new CreateProjectCommand { Title = "Test Project Title 2" });
        Assert.Equal(HttpStatusCode.Created, responseCreateProject2.StatusCode);
        
        var createdProject2 = await responseCreateProject2.Content.ReadFromJsonAsync<ProjectResponse>();
        Assert.NotNull(createdProject2);

        // Step 2: Create two tickets with different projects and assignees
        var createTicketCommand1 = new CreateTicketCommand
        {
            Title = "Ticket 1, Project 1, Assignee 1",
            ProjectId = createdProject1.Id,
            Assignee = 1 
        };
        
        var createTicketCommand2 = new CreateTicketCommand
        {
            Title = "Ticket 2, Project 2, Assignee 2",
            ProjectId = createdProject2.Id,
            Assignee = 2
        };

        var responseCreateTicket1 = await _client.PostAsJsonAsync("/api/ticket", createTicketCommand1);
        var responseCreateTicket2 = await _client.PostAsJsonAsync("/api/ticket", createTicketCommand2);
        Assert.Equal(HttpStatusCode.Created, responseCreateTicket1.StatusCode);
        Assert.Equal(HttpStatusCode.Created, responseCreateTicket2.StatusCode);

        // Step 3: Run filters
        var ticketsInProject1AndAssignee1 = 
            await _client.GetFromJsonAsync<PagedResult<TicketResponse>>($"/api/ticket?projects={createdProject1.Id}&assignees=1");

        var ticketsInProject2AndAssignee2 = 
            await _client.GetFromJsonAsync<PagedResult<TicketResponse>>($"/api/ticket?projects={createdProject2.Id}&assignees=2");
        
        var ticketsInProject1AndAssignee2 = 
            await _client.GetFromJsonAsync<PagedResult<TicketResponse>>($"/api/ticket?projects={createdProject1.Id}&assignees=2");
        
        // Step 4: Assert
        Assert.NotNull(ticketsInProject1AndAssignee1);
        Assert.Contains(ticketsInProject1AndAssignee1.Items, t => t.Title == createTicketCommand1.Title);
        Assert.All(ticketsInProject1AndAssignee1.Items, t => Assert.Equal(1, t.Assignee?.Id));
        Assert.DoesNotContain(ticketsInProject1AndAssignee1.Items, t => t.Title == createTicketCommand2.Title);

        Assert.NotNull(ticketsInProject2AndAssignee2);
        Assert.Contains(ticketsInProject2AndAssignee2.Items, t => t.Title == createTicketCommand2.Title);
        Assert.All(ticketsInProject2AndAssignee2.Items, t => Assert.Equal(2, t.Assignee?.Id));
        Assert.DoesNotContain(ticketsInProject2AndAssignee2.Items, t => t.Title == createTicketCommand1.Title);
        
        Assert.NotNull(ticketsInProject1AndAssignee2);
        Assert.DoesNotContain(ticketsInProject1AndAssignee2.Items, t => t.Title == createTicketCommand1.Title);
        Assert.DoesNotContain(ticketsInProject1AndAssignee2.Items, t => t.Title == createTicketCommand2.Title);
        
        // Step 5: Cleanup
        var responseDeleteProject = await _client.DeleteAsync($"/api/project/{createdProject2.Id}");
        Assert.Equal(HttpStatusCode.NoContent, responseDeleteProject.StatusCode);
    }
}