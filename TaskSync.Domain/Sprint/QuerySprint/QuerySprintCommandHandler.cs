using TaskSync.Domain.Shared;
using TaskSync.Domain.Ticket;
using TaskSync.Domain.Ticket.QueryTicket;

namespace TaskSync.Domain.Sprint.QuerySprint;

public class QuerySprintCommandHandler : ICommandHandler
{
    private readonly ISprintRepository _sprintRepository;
    private readonly ITicketRepository _ticketRepository;

    public QuerySprintCommandHandler(ISprintRepository sprintRepository, ITicketRepository ticketRepository)
    {
        _sprintRepository = sprintRepository;
        _ticketRepository = ticketRepository;
    }

    public async Task<Result<SprintModel>> GetDraftSprintAsync(int projectId, CancellationToken cancellationToken)
    {
        var sprint = await _sprintRepository.GetDraftSprintAsync(projectId, cancellationToken);
        if (!sprint.Success)
        {
            return sprint;
        }
        
        var tickets = await _ticketRepository.GetAllAsync(new TicketSearchFilter
        {
            BoardId = sprint.Value!.Id,
            OrderBy = TicketModel.OrderByPosition,
        }, cancellationToken);

        sprint.Value.Tickets = tickets;
        
        return sprint;
    }
}