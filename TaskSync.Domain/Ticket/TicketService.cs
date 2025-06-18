using TaskSync.Domain.Exceptions;
using TaskSync.Domain.Project;
using TaskSync.Domain.Shared;
using TaskSync.Domain.Ticket.Command;

namespace TaskSync.Domain.Ticket;

public class TicketService : ITicketService
{
    private readonly ITicketRepository _ticketRepository;
    private readonly IProjectRepository _projectRepository;

    public TicketService(ITicketRepository taskRepository, IProjectRepository projectRepository)
    {
        _ticketRepository = taskRepository;
        _projectRepository = projectRepository;
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
}
