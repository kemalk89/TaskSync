using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskSync.Controllers.Request;
using TaskSync.Controllers.Response;
using TaskSync.Domain.Exceptions;
using TaskSync.Domain.Shared;
using TaskSync.Domain.Ticket;

namespace TaskSync.Controllers;

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
        PagedResult<TicketModel> pagedResult = await _ticketService.GetTicketsAsync(pageNumber, pageSize);
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

    [HttpPost]
    [Route("{id}/comment")]
    public async Task<ActionResult<TicketCommentResponse>> AddComment([FromRoute] int id, [FromBody] CreateTicketCommentRequest req)
    {
        try
        {
            var comment = await _ticketService.AddCommentAsync(id, req.ToCommand());
            var commentResponse = new TicketCommentResponse(comment);
            return new ObjectResult(commentResponse) { StatusCode = StatusCodes.Status201Created };
        }   
        catch (ResourceNotFoundException e)
        {
            return NotFound($"Ticket with id {id} could not be found.");
        }
    }
    
    [HttpGet]
    [Route("{id}/comment")]
    public async Task<PagedResult<TicketCommentResponse>> GetTicketComments(
        [FromRoute] int id, 
        [FromQuery] int pageNumber = 1, 
        [FromQuery] int pageSize = 50)
    {
        var paged = await _ticketService.GetTicketCommentsAsync(id, pageNumber, pageSize);
        return new PagedResult<TicketCommentResponse>
        {
            PageNumber = paged.PageNumber,
            PageSize = paged.PageSize,
            Items = paged.Items.Select(item => new TicketCommentResponse(item)),
            Total = paged.Total
        };
    }
}
