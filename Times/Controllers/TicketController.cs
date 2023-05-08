using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Times.Controllers.Request;
using Times.Controllers.Response;
using Times.Domain.Exceptions;
using Times.Domain.Shared;
using Times.Domain.Ticket;

namespace Times.Controllers;

[Authorize]
[ApiController]
[Route("/api/[controller]")]
public class TicketController : ControllerBase
{
    private readonly ILogger<TicketController> _logger;

    private readonly ITicketService _ticketService;

    public TicketController(ITicketService ticketService, ILogger<TicketController> logger)
    {
        _ticketService = ticketService;
        _logger = logger;
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
            if (ticket == null)
            {
                return BadRequest("Ticket could not be created.");
            }
            else
            {
                return CreatedAtAction(
                    nameof(GetTicketById),
                    new { id = ticket.Id },
                    new TicketResponse(ticket));
            }
        }
        catch (DomainException e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpPatch]
    [Route("{id}/status/{statusId}")]
    public async Task<ActionResult> UpdateTicketStatus([FromRoute] int id, [FromRoute] int statusId)
    {
        try
        {
            var newStatus = await _ticketService.UpdateTicketStatusAsync(id, statusId);
            return Ok(newStatus);
        }
        catch (ResourceNotFoundException e)
        {
            _logger.LogError(e.Message);
            return NotFound("Ticket or status not found.");
        }

    }
}
