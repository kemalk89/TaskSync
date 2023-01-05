using Microsoft.AspNetCore.Mvc;
using Times.Domain.Ticket;
using Times.Infrastructure.Entities;

namespace Times.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class TicketController : ControllerBase
{

    ITicketService _ticketService;

    public TicketController(ITicketService ticketService)
    {
        _ticketService = ticketService;
    }

    [HttpGet]
    public async Task<IEnumerable<TicketResponse>> GetTickets()
    {
        IEnumerable<Ticket> tickets = await _ticketService.GetTicketsAsync();
        return tickets.Select(t => new TicketResponse(t));
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<TicketResponse> GetTicketById([FromRoute] int id)
    {
        var ticket = await _ticketService.GetTicketByIdAsync(id);
        return new TicketResponse(ticket);
    }

    [HttpPost]
    public async Task<TicketResponse> CreateTicket([FromBody] CreateTicketRequest req)
    {
        var ticket = await _ticketService.CreateTicketAsync(req.Title, req.Description);
        return new TicketResponse(ticket);
    }
}
