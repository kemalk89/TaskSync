using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskSync.Controllers.Request;
using TaskSync.Controllers.Response;
using TaskSync.Domain.Exceptions;
using TaskSync.Domain.Shared;
using TaskSync.Domain.Ticket;
using TaskSync.Domain.Ticket.AddTicketComment;
using TaskSync.Domain.Ticket.AssignTicketLabel;
using TaskSync.Domain.Ticket.CreateTicket;
using TaskSync.Domain.Ticket.DeleteTicket;
using TaskSync.Domain.Ticket.DeleteTicketComment;
using TaskSync.Domain.Ticket.QueryTicket;
using TaskSync.Domain.Ticket.UpdateTicket;

namespace TaskSync.Controllers;

[Authorize]
[ApiController]
[Route("/api/[controller]")]
public class TicketController : ControllerBase
{
    private readonly QueryTicketCommandHandler _queryTicketCommandHandler;
    private readonly CreateTicketCommandHandler _createTicketCommandHandler;
    private readonly UpdateTicketCommandHandler _updateTicketCommandHandler;
    private readonly DeleteTicketCommandHandler _deleteTicketCommandHandler;
    private readonly DeleteTicketCommentCommandHandler _deleteTicketCommentCommandHandler;
    private readonly AssignTicketLabelCommandHandler _assignTicketLabelCommandHandler;
    private readonly AddTicketCommentCommandHandler _addTicketCommentCommandHandler;
    public TicketController(
        DeleteTicketCommandHandler deleteTicketCommandHandler, 
        UpdateTicketCommandHandler updateTicketCommandHandler, 
        CreateTicketCommandHandler createTicketCommandHandler, 
        AssignTicketLabelCommandHandler assignTicketLabelCommandHandler, 
        DeleteTicketCommentCommandHandler deleteTicketCommentCommandHandler, 
        QueryTicketCommandHandler queryTicketCommandHandler, 
        AddTicketCommentCommandHandler addTicketCommentCommandHandler)
    {
        _deleteTicketCommandHandler = deleteTicketCommandHandler;
        _updateTicketCommandHandler = updateTicketCommandHandler;
        _createTicketCommandHandler = createTicketCommandHandler;
        _assignTicketLabelCommandHandler = assignTicketLabelCommandHandler;
        _deleteTicketCommentCommandHandler = deleteTicketCommentCommandHandler;
        _queryTicketCommandHandler = queryTicketCommandHandler;
        _addTicketCommentCommandHandler = addTicketCommentCommandHandler;
    }

    [HttpGet]
    public async Task<PagedResult<TicketResponse>> GetTickets([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 50, [FromQuery] string? searchText = null)
    {
        var searchFilter = new TicketSearchFilter { SearchText = searchText ?? string.Empty };
        PagedResult<TicketModel> pagedResult = await _queryTicketCommandHandler.GetTicketsAsync(pageNumber, pageSize, searchFilter);
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
        var ticket = await _queryTicketCommandHandler.GetTicketByIdAsync(id);
        if (ticket == null)
        {
            return NotFound("Ticket not found");
        }
        return Ok(new TicketResponse(ticket));
    }
    
    [HttpPost("{ticketId}/labels")]
    public async Task<ActionResult<bool>> AssignTicketLabel(int ticketId, [FromBody] AssignTicketLabelCommand cmd)
    {
        var result = await _assignTicketLabelCommandHandler.HandleAsync(ticketId, cmd);

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
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<CreateTicketResponse>> CreateTicket([FromBody] CreateTicketCommand command)
    {
        var result = await _createTicketCommandHandler.HandleAsync(command);
        if (result.Success)
        {
            return CreatedAtAction(
                nameof(GetTicketById),
                new { id = result.Value },
                new CreateTicketResponse { TicketId = result.Value});
        }

        return result.Error switch
        {
            ResultCodes.ResultCodeResourceNotFound => NotFound(new ErrorResponse(result.Error, result.ErrorDetails)),
            ResultCodes.ResultCodeValidationFailed => BadRequest(new ErrorResponse(result.Error, result.ErrorDetails)),
            _ => throw new InvalidOperationException($"Unexpected result code: {result.Error}.")
        };
    }

    [HttpPatch]
    [Route("{id}")]
    public async Task<ActionResult<Result<bool>>> UpdateTicket([FromRoute] int id, [FromBody] UpdateTicketCommand updateTicketCommand)
    { 
        var newStatus = await _updateTicketCommandHandler.HandleAsync(id, updateTicketCommand);
        return Ok(newStatus);
    }

    [HttpPost]
    [Route("{id}/comment")]
    public async Task<ActionResult<TicketCommentResponse>> AddComment([FromRoute] int id, [FromBody] CreateTicketCommentRequest req)
    {
        var result = await _addTicketCommentCommandHandler.HandleAsync(id, req.ToCommand());

        if (result.Success)
        {
            return CreatedAtAction(null, new { id }, req);
        }
        
        return result.Error switch
        {
            ResultCodes.ResultCodeResourceNotFound => NotFound(new ErrorResponse(result.Error, result.ErrorDetails)),
            ResultCodes.ResultCodeValidationFailed => BadRequest(new ErrorResponse(result.Error, result.ErrorDetails)),
            _ => throw new InvalidOperationException($"Unexpected result code: {result.Error}.")
        };
    }
    
    [HttpGet]
    [Route("{id}/comment")]
    public async Task<PagedResult<TicketCommentResponse>> GetTicketComments(
        [FromRoute] int id, 
        [FromQuery] int pageNumber = 1, 
        [FromQuery] int pageSize = 50)
    {
        var paged = await _queryTicketCommandHandler.GetTicketCommentsAsync(id, pageNumber, pageSize);
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
        var result = await _deleteTicketCommandHandler.HandleAsync(id);
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
        var result = await _deleteTicketCommentCommandHandler.HandleAsync(commentId);
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

    [HttpGet]
    [Route("status")]
    public async Task<ActionResult> GetTicketStatusList(CancellationToken cancellationToken)
    {
        var result =await _queryTicketCommandHandler.GetTicketStatusListAsync(cancellationToken);  
        return Ok(result);
    }
}
