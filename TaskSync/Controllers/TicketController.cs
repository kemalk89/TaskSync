using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskSync.Controllers.Request;
using TaskSync.Controllers.Response;
using TaskSync.Domain.Exceptions;
using TaskSync.Domain.Project.Commands;
using TaskSync.Domain.Shared;
using TaskSync.Domain.Ticket;
using TaskSync.Domain.Ticket.Command;

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
    public async Task<PagedResult<TicketResponse>> GetTickets([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 50, [FromQuery] string? searchText = null)
    {
        var searchFilter = new TicketSearchFilter { SearchText = searchText };
        PagedResult<TicketModel> pagedResult = await _ticketService.GetTicketsAsync(pageNumber, pageSize, searchFilter);
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
            return NotFound("Ticket not found");
        }
        return Ok(new TicketResponse(ticket));
    }
    
    [HttpPost("{ticketId}/labels")]
    public async Task<ActionResult<bool>> AssignTicketLabel(int ticketId, [FromBody] AssignTicketLabelCommand cmd)
    {
        var result = await _ticketService.AssignTicketLabelAsync(ticketId, cmd);

        return result switch
        {
            { Success: false, Error: ResultCodes.ResultCodeResourceNotFound } => NotFound(result.ErrorDetails),
            { Success: false, Error: ResultCodes.ResultCodeValidationFailed } => BadRequest(result.ErrorDetails),
            _ => Ok(result.Value)
        };
    }

    /*
    [HttpPost("{ticketId}/labels")]
    public async Task<ActionResult> UnassignTicketLabel([FromRoute] int ticketId)
    {
        // TODO
        throw new NotImplementedException();
    }
    */
    [HttpPost]
    public async Task<ActionResult<CreateTicketResponse>> CreateTicket([FromBody] CreateTicketRequest req)
    {
        try
        {
            var ticketId = await _ticketService.CreateTicketAsync(req.ToCommand());
            if (ticketId == null)
            {
                return BadRequest("Ticket could not be created.");
            }
            
            return CreatedAtAction(
                nameof(GetTicketById),
                new { id = ticketId },
                new CreateTicketResponse { TicketId = ticketId.Value});
        }
        catch (DomainException e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpPatch]
    [Route("{id}")]
    public async Task<ActionResult<Result<bool>>> UpdateTicket([FromRoute] int id, [FromBody] UpdateTicketCommand updateTicketCommand)
    { 
        var newStatus = await _ticketService.UpdateTicketAsync(id, updateTicketCommand);
        return Ok(newStatus);
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
        catch (ResourceNotFoundException)
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

    [HttpDelete]
    [Route("{id}")]
    public async Task<ActionResult> DeleteTicket([FromRoute] int id)
    {
        // TODO Add Email notification
        var result = await _ticketService.DeleteTicketAsync(id);
        if (result.Success)
        {
            return NoContent();
        }
        
        return StatusCode(
            StatusCodes.Status403Forbidden, 
            new
            {
                message = "You do not have permission to delete this ticket."
            });
    }

    [HttpDelete]
    [Route("{id}/comment/{commentId}")]
    public async Task<ActionResult> DeleteTicketComment([FromRoute] int id, [FromRoute] int commentId)
    {
        var result = await _ticketService.DeleteTicketCommentAsync(commentId);
        if (result.Success)
        {
            return NoContent();
        }

        return StatusCode(
            StatusCodes.Status404NotFound, 
            new
            {
                message = "Unable to delete comment with ID " +  commentId
            });
    }
}
