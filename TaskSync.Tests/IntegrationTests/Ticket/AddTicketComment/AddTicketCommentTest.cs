using System.Net;
using System.Net.Http.Json;

using TaskSync.Controllers.Request;
using TaskSync.Controllers.Response;
using TaskSync.Domain.Shared;

namespace TaskSync.Tests.IntegrationTests.Ticket.AddTicketComment;

public class AddTicketCommentTest : BaseIntegrationTest
{
    public AddTicketCommentTest(IntegrationTestWebAppFactory factory) : base(factory)
    {
    }
    
    [Fact]
    public async Task AddTicketComment_ShouldReturn401_WhenNoAuthProvided()
    {
        await AssertEndpointsReturnUnauthorized([
            ("/api/ticket/1/comment", HttpMethod.Post, new CreateTicketCommentRequest("Any comment"))
        ]);
    }

    [Fact]
    public async Task AddTicketComment_ShouldReturn404_WhenNoTicketExists()
    {
        SetAuthenticatedUser();
        
        var response = await _client.PostAsJsonAsync(
            "/api/ticket/100000/comment", new CreateTicketCommentRequest("Any comment"));
        
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        var errors = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        Assert.Equal(ResultCodes.ResultCodeResourceNotFound, errors?.ErrorCode);
        Assert.Equal("No ticket found with ID 100000.", errors?.ErrorDetails[0]);
    }
    
        
    [Fact]
    public async Task CreateTicket_ShouldReturn400_WhenInvalidRequestProvided()
    {
        SetAuthenticatedUser();
        
        var response = await _client.PostAsJsonAsync(
            "/api/ticket/100000/comment", new CreateTicketCommentRequest(""));
        
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var errors = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        Assert.Equal(ResultCodes.ResultCodeValidationFailed, errors?.ErrorCode);
        Assert.Equal("'Comment' must not be empty.", errors?.ErrorDetails[0]);
    }
}