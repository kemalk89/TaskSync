using System.Net;
using System.Net.Http.Json;

using TaskSync.Controllers.Response;
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
}