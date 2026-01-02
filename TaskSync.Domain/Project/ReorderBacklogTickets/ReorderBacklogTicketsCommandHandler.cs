using TaskSync.Domain.Shared;
using TaskSync.Domain.Ticket;
using TaskSync.Domain.Ticket.QueryTicket;

namespace TaskSync.Domain.Project.ReorderBacklogTickets;

public class ReorderBacklogTicketsCommandHandler : ICommandHandler
{
    private readonly ITicketRepository _ticketRepository;

    public ReorderBacklogTicketsCommandHandler(ITicketRepository ticketRepository)
    {
        _ticketRepository = ticketRepository;
    }

    public async Task<Result<int>> HandleAsync(int projectId, List<ReorderBacklogTicketCommand> ticketOrder, CancellationToken cancellationToken)
    {
        // Validation 1
        if (ticketOrder.Count == 0)
        {
            return Result<int>.Fail("No tickets to reorder.");
        }
        
        // Validation 2
        var ticketIds = ticketOrder.Select(i => i.TicketId);
        var filter = new TicketSearchFilter { ProjectIds = [projectId], TicketIds = ticketIds.ToList() };
        var foundTickets = await _ticketRepository.GetAllAsync(filter, cancellationToken);
        if (foundTickets.Count != ticketOrder.Count)
        {
            return Result<int>.Fail("One or more tickets were not found.");
        }
        
        // Update
        var result = 
            await _ticketRepository.ReorderBacklogTickets(projectId, ticketOrder, cancellationToken);

        return result;
    }
}