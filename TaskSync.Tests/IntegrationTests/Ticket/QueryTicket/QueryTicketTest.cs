using System.Net;
using System.Text.Json;

using TaskSync.Controllers.Response;
using TaskSync.Domain.Shared;
using TaskSync.Domain.Ticket;

namespace TaskSync.Tests.IntegrationTests.Ticket.QueryTicket;

public class QueryTicketTest : BaseIntegrationTest
{
    public QueryTicketTest(IntegrationTestWebAppFactory factory) : base(factory)
    {
    }
    
    [Fact]
    public async Task GetTickets_ShouldReturn200()
    {
        SetAuthenticatedUser();
        
        var responseGet = await _client.GetAsync("/api/ticket");
        
        Assert.Equal(HttpStatusCode.OK, responseGet.StatusCode);
        
        var json = await responseGet.Content.ReadAsStringAsync();
        var list = JsonSerializer.Deserialize<PagedResult<TicketResponse>>(json);
        Assert.NotNull(list);
    }
    
    [Fact]
    public async Task GetTicketById_ShouldReturn404()
    {
        SetAuthenticatedUser();
        
        var responseGet = await _client.GetAsync("/api/ticket/100000");
        
        Assert.Equal(HttpStatusCode.NotFound, responseGet.StatusCode);
    }
    
    [Fact]
    public async Task GetTicketStatusList_ShouldReturn200()
    {
        SetAuthenticatedUser();
        
        var responseGet = await _client.GetAsync("/api/ticket/status");
        
        Assert.Equal(HttpStatusCode.OK, responseGet.StatusCode);
        
        var json = await responseGet.Content.ReadAsStringAsync();
        var list = JsonSerializer.Deserialize<List<TicketStatusModel>>(json);
        Assert.NotNull(list);
        Assert.Equal(3, list.Count);
    }
}