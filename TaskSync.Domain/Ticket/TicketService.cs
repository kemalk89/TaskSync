using FluentValidation;

using TaskSync.Domain.Exceptions;
using TaskSync.Domain.Project;
using TaskSync.Domain.Project.Commands;
using TaskSync.Domain.Shared;
using TaskSync.Domain.Ticket.Command;
using TaskSync.Domain.User;

namespace TaskSync.Domain.Ticket;

public class TicketService : ITicketService
{
    private readonly ITicketRepository _ticketRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IValidator<CreateTicketCommand> _createTicketCommandValidator;
    private readonly IValidator<AssignTicketLabelCommand> _assignTicketLabelCommandValidator;

    public TicketService(
        ITicketRepository taskRepository,
        IProjectRepository projectRepository, 
        ICurrentUserService currentUserService, 
        IValidator<CreateTicketCommand> createTicketCommandValidator, 
        IValidator<AssignTicketLabelCommand> assignTicketLabelCommandValidator
    )
    {
        _ticketRepository = taskRepository;
        _projectRepository = projectRepository;
        _currentUserService = currentUserService;
        _createTicketCommandValidator = createTicketCommandValidator;
        _assignTicketLabelCommandValidator = assignTicketLabelCommandValidator;
    }

    public async Task<Result<int>> CreateTicketAsync(CreateTicketCommand cmd)
    {
        var result = await _createTicketCommandValidator.ValidateAsync(cmd);
        if (!result.IsValid)
        {
            return Result<int>.Fail(ResultCodes.ResultCodeValidationFailed, result);
        }
        
        var project = await _projectRepository.GetByIdAsync(cmd.ProjectId);
        if (project == null)
        {
            return Result<int>.Fail(ResultCodes.ResultCodeResourceNotFound, result);
        }
        
        // Create the new ticket
        var ticketId = await _ticketRepository.CreateAsync(cmd);
        if (ticketId == null)
        {
            throw new DomainException("Could not create ticket");
        }
        
        // Assign Labels
        if (cmd.Labels?.Count > 0)
        {
            foreach (var newLabel in cmd.Labels)
            {
                var assignTicketLabelResult = await AssignTicketLabelAsync(cmd.ProjectId, ticketId.Value, newLabel);
                if (!assignTicketLabelResult.Success)
                {
                    return assignTicketLabelResult;
                }
            }
            
        }
        
        return Result<int>.Ok(ticketId.Value);
    }

    public async Task<TicketModel?> GetTicketByIdAsync(int id)
    {
        var ticket = await _ticketRepository.GetByIdAsync(id);
        return ticket;
    }

    public async Task<PagedResult<TicketModel>> GetTicketsAsync(int pageNumber, int pageSize, TicketSearchFilter filter)
    {
        var tickets = await _ticketRepository.GetAllAsync(pageNumber, pageSize, filter);
        return tickets;
    }

    public async Task<PagedResult<TicketModel>> GetTicketsByProjectIdAsync(int projectId, int pageNumber, int pageSize)
    {
        return await _ticketRepository.GetByProjectIdAsync(projectId, pageNumber, pageSize);
    }

    public async Task<TicketCommentModel> AddCommentAsync(int id, CreateTicketCommentCommand cmd)
    {
        var ticket = await _ticketRepository.GetByIdAsync(id);
        if (ticket == null)
        {
            throw new ResourceNotFoundException($"No ticket found with ID {id}.");
        }

        var comment = await _ticketRepository.AddTicketCommentAsync(id, cmd);
        return comment;
    }

    public async Task<PagedResult<TicketCommentModel>> GetTicketCommentsAsync(int id, int pageNumber, int pageSize)
    {
        return await _ticketRepository.GetTicketCommentsAsync(id, pageNumber, pageSize);
    }

    public async Task<Result<bool>> DeleteTicketAsync(int id)
    {
        // Check if the current user is an admin, or part of the project team or is author of the ticket
        var ticket = await _ticketRepository.GetByIdAsync(id);
        if (ticket is null)
        {
            return Result<bool>.Fail("No ticket found with ID " + id);
        }
        
        // TODO if the current user has role ADMIN, allow deletion
        
        // TODO if the current user is part of the project team, allow deletion
        // TODO var project = ticket.Project;

        // if the current user is author of the ticket, allow deletion
        var author = ticket.CreatedBy;
        if (author != null)
        {
            var currentUser = await _currentUserService.GetCurrentUserAsync();
            if (currentUser?.Id == author.Id)
            {
                await _ticketRepository.DeleteTicketAsync(id);
                return Result<bool>.Ok(true);
            }
        }

        return Result<bool>.Fail("Could not delete the ticket with ID " + id);
    }

    public async Task<Result<bool>> DeleteTicketCommentAsync(int commentId)
    {
        var comment = await _ticketRepository.GetTicketCommentByIdAsync(commentId);
        if (comment == null)
        {
            return Result<bool>.Fail("Could not delete the ticket comment with ID " + commentId + ". Not found.");
        }
        
        // if the current user is author of the comment, allow deletion
        var authorId = comment.CreatedById;
        var currentUser = await _currentUserService.GetCurrentUserAsync();
        if (currentUser?.Id == authorId)
        {
            var result = await _ticketRepository.DeleteTicketCommentAsync(commentId);
            return result ? Result<bool>.Ok(result) : Result<bool>.Fail("Could not delete the ticket comment with ID " + commentId);
        }
        
        return Result<bool>.Fail("Cannot delete comment with ID " + commentId + ". No permissions.");
    }

    public async Task<Result<bool>> UpdateTicketAsync(int ticketId, UpdateTicketCommand updateTicketCommand)
    {   
        var result = await _ticketRepository.UpdateTicketAsync(ticketId, updateTicketCommand);
        return result;
    }

    public async Task<Result<int>> AssignTicketLabelAsync(int projectId, int ticketId, AssignTicketLabelCommand cmd)
    {
        // Validation
        var validationResult = await _assignTicketLabelCommandValidator.ValidateAsync(cmd);
        if (!validationResult.IsValid)
        {
            return Result<int>.Fail(ResultCodes.ResultCodeValidationFailed, validationResult);
        }
        
        var ticket = await _ticketRepository.GetByIdAsync(ticketId);
        if (ticket is null)
        {
            return Result<int>.Fail(ResultCodes.ResultCodeResourceNotFound, ErrorDetails.TicketNotFound);
        }
        
        var labelId = cmd.LabelId;
        if (labelId != null)
        {
            var alreadyAssigned = ticket.Labels.Find(i => i.Id == labelId) != null;
            if (alreadyAssigned)
            {
                return Result<int>.Fail(
                    ResultCodes.ResultCodeValidationFailed,
                    $"Label ID {labelId} already assigned to ticket.");
            }             
        }
        
        // Create new label if needed for the project
        if (labelId == null && !string.IsNullOrWhiteSpace(cmd.Title))
        {
            labelId = await _projectRepository.CreateLabelAsync(projectId, cmd.Title);
        }

        if (labelId == null)
        {
            throw new DomainException("Unknown Domain Exception.");
        }
        
        // Assign label to ticket
        var assignLabelResult = await _ticketRepository.AssignTicketLabelAsync(projectId, ticketId, labelId.Value);
        return assignLabelResult;
    }
}
