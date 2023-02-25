using Times.Domain.Project;
using Times.Domain.Shared;
using Times.Domain.Ticket.Command;

namespace Times.Domain.Ticket;

public class TicketService : ITicketService
{
    private readonly ITicketRepository _ticketRepository;
    private readonly IProjectRepository _projectRepository;

    public TicketService(ITicketRepository taskRepository, IProjectRepository projectRepository)
    {
        _ticketRepository = taskRepository;
        _projectRepository = projectRepository;
    }

    public async Task<Ticket> CreateTicketAsync(CreateTicketCommand cmd)
    {
        var project = await _projectRepository.GetByIdAsync(cmd.ProjectId);
        if (project == null)
        {
            throw new DomainException($"Trying to create a ticket for project with ID {cmd.ProjectId}. This project does not exist.");
        }

        var ticket = await _ticketRepository.CreateAsync(cmd);
        return ticket;
    }

    public async Task<Ticket?> GetTicketByIdAsync(int id)
    {
        var ticket = await _ticketRepository.GetByIdAsync(id);
        return ticket;
    }

    public async Task<PagedResult<Ticket>> GetTicketsAsync(int pageNumber, int pageSize)
    {
        var tickets = await _ticketRepository.GetAllAsync(pageNumber, pageSize);
        return tickets;
    }
}
