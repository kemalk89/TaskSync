using TaskSync.Domain.Exceptions;
using TaskSync.Domain.Project;
using TaskSync.Domain.Shared;
using TaskSync.Domain.Ticket.Command;
using TaskSync.Domain.User;

namespace TaskSync.Domain.Ticket;

public class TicketService : ITicketService
{
    private readonly ITicketRepository _ticketRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly ICurrentUserService _currentUserService;

    public TicketService(ITicketRepository taskRepository, IProjectRepository projectRepository, ICurrentUserService currentUserService)
    {
        _ticketRepository = taskRepository;
        _projectRepository = projectRepository;
        _currentUserService = currentUserService;
    }

    public async Task<int?> CreateTicketAsync(CreateTicketCommand cmd)
    {
        var project = await _projectRepository.GetByIdAsync(cmd.ProjectId);
        if (project == null)
        {
            throw new DomainException($"Trying to create a ticket for project with ID {cmd.ProjectId}. This project does not exist.");
        }

        var ticketId = await _ticketRepository.CreateAsync(cmd);
        return ticketId;
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

    public async Task<TicketStatus> UpdateTicketStatusAsync(int ticketId, int statusId)
    {
        var newStatus = await _ticketRepository.UpdateTicketStatusAsync(ticketId, statusId);
        return newStatus;
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
}
