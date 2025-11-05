using System.Net;
using System.Net.Http.Json;

using TaskSync.Controllers.Response;
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
    public async Task FilterTickets()
    {
        SetAuthenticatedUser();
        
        // Arrange
        var createdProject = await _createProjectFixture.InitIfNotExistsAsync(
            _client, new CreateProjectCommand() { Title = "Test Project Title" });
        
        Assert.NotNull(createdProject);
        
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
        
        // Act
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
}