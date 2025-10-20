using System.Net;
using System.Net.Http.Json;

using TaskSync.Controllers.Request;
using TaskSync.Controllers.Response;
using TaskSync.Domain.Project.Commands;
using TaskSync.Domain.Ticket.Command;
using TaskSync.Domain.Shared;

namespace TaskSync.Tests.IntegrationTests;

public class TicketControllerTest : BaseIntegrationTest
{
    public TicketControllerTest(IntegrationTestWebAppFactory factory) : base(factory)
    {
    }
    
    [Fact]
    public async Task Api_ShouldReturn401_WhenNoAuthProvided()
    {
        await AssertEndpointsReturnUnauthorized([
            ("/api/ticket/123", HttpMethod.Get, null),
            ("/api/ticket", HttpMethod.Get, null),
            ("/api/ticket", HttpMethod.Post, new CreateTicketCommand())
        ]);
    }
    
    [Fact]
    public async Task CreateTicket_ShouldReturn400_WhenInvalidRequestProvided()
    {
        SetAuthenticatedUser();
        
        var cmd = new CreateTicketCommand();
        
        var responseCreate = await _client.PostAsJsonAsync("/api/ticket", cmd);
        
        Assert.Equal(HttpStatusCode.BadRequest, responseCreate.StatusCode);
        
        var errors = await responseCreate.Content.ReadFromJsonAsync<ErrorResponse>();
        Assert.Equal(ResultCodes.ResultCodeValidationFailed, errors?.ErrorCode);
        Assert.Equal( "'Title' must not be empty.", errors?.ErrorDetails[0]);
        Assert.Equal( "'Project Id' must not be empty.", errors?.ErrorDetails[1]);
    }
    
    [Fact]
    public async Task CreateTicket_ShouldReturn404_WhenNoProjectExists()
    {
        SetAuthenticatedUser();
        
        var cmd = new CreateTicketCommand
        {
            Title = "Test Ticket",
            ProjectId = 123456789
        };
        
        var responseCreate = await _client.PostAsJsonAsync("/api/ticket", cmd);
        
        Assert.Equal(HttpStatusCode.NotFound, responseCreate.StatusCode);
        
        var errors = await responseCreate.Content.ReadFromJsonAsync<ErrorResponse>();
        Assert.Equal(ResultCodes.ResultCodeResourceNotFound, errors?.ErrorCode);
    }

    [Fact]
    public async Task CreateTicket_ShouldReturn201_WhenValidRequest()
    {
        SetAuthenticatedUser();
        
        // first, create a project
        var createProjectCommand = new CreateProjectCommand
        {
            Title = "Test Project Title"
        };
        
        var responseCreateProject = await _client.PostAsJsonAsync("/api/project", createProjectCommand);
        
        Assert.Equal(HttpStatusCode.Created, responseCreateProject.StatusCode);
        
        var createdProject = await responseCreateProject.Content.ReadFromJsonAsync<ProjectResponse>();
        Assert.NotNull(createdProject);
        
        // next, create a label for the project
        var responseCreateLabel = await _client.PostAsJsonAsync(
            $"/api/project/{createdProject.Id}/labels", 
            new CreateProjectLabelCommand { ProjectId = createdProject.Id, Text = "Quick Fix" });
        
        Assert.Equal(HttpStatusCode.Created, responseCreateLabel.StatusCode);
        var createdLabel = await responseCreateLabel.Content.ReadFromJsonAsync<ProjectLabelResponse>();
        Assert.NotNull(createdLabel);
        
        // next, create a ticket with 2 labels
        var createTicketCommand = new CreateTicketCommand
        {
            Title = "Test Ticket",
            ProjectId = createdProject!.Id,
            Labels = [
                new AssignTicketLabelCommand { Title = "Team A" }, // Create and assign new ticket label
                new AssignTicketLabelCommand { LabelId = createdLabel.Id } // Assign existing ticket label: Label with ID 3 is existing one, which has been seeded 
            ]
        };
        
        var responseCreateTicket = await _client.PostAsJsonAsync("/api/ticket", createTicketCommand);
        Assert.Equal(HttpStatusCode.Created, responseCreateTicket.StatusCode);
        
        // now, verify the project has really 2 labels assigned
        var responseGetLabels = 
            await _client.GetFromJsonAsync<List<ProjectLabelResponse>>($"/api/project/{createdProject.Id}/labels");
        Assert.NotNull(responseGetLabels);
        Assert.Equal(2, responseGetLabels.Count);
        Assert.Equal("Quick Fix", responseGetLabels.First().Text);
        Assert.Equal("Team A", responseGetLabels.Last().Text);
    }
}