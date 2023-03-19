using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Times.Controllers.Request;
using Times.Controllers.Response;
using Times.Domain;
using Times.Domain.Shared;
using Times.Domain.Ticket;

namespace Times.Controllers;

[Authorize]
[ApiController]
[Route("/api/[controller]")]
public class TicketController : ControllerBase
{

    private readonly ITicketService _ticketService;

    public TicketController(ITicketService ticketService)
    {
        _ticketService = ticketService;
    }

    [HttpGet]
    public async Task<PagedResult<TicketResponse>> GetTickets([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 50)
    {
        PagedResult<Ticket> pagedResult = await _ticketService.GetTicketsAsync(pageNumber, pageSize);
        return new PagedResult<TicketResponse>
        {
            PageNumber = pagedResult.PageNumber,
            PageSize = pagedResult.PageSize,
            Items = pagedResult.Items.Select(item => new TicketResponse(item)),
            Total = pagedResult.Total
        };
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<ActionResult<TicketResponse>> GetTicketById([FromRoute] int id)
    {
        var ticket = await _ticketService.GetTicketByIdAsync(id);
        if (ticket == null)
        {
            return NotFound();
        }
        return Ok(new TicketResponse(ticket));
    }

    [HttpPost]
    public async Task<ActionResult<TicketResponse>> CreateTicket([FromBody] CreateTicketRequest req)
    {
        try
        {
            var ticket = await _ticketService.CreateTicketAsync(req.ToCommand());
            return CreatedAtAction(
                nameof(GetTicketById),
                new { id = ticket.Id },
                new TicketResponse(ticket));
        }
        catch (DomainException e)
        {
            return NotFound(e.Message);
        }

    }
}
