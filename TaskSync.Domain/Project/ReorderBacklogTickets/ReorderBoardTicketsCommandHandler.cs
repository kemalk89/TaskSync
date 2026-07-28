using TaskSync.Domain.Shared;
using TaskSync.Domain.Ticket;
using TaskSync.Domain.Ticket.QueryTicket;

namespace TaskSync.Domain.Project.ReorderBacklogTickets;

public class ReorderBoardTicketsCommandHandler : ICommandHandler
{
    private readonly ITicketRepository _ticketRepository;

    public ReorderBoardTicketsCommandHandler(ITicketRepository ticketRepository)
    {
        _ticketRepository = ticketRepository;
    }

    public async Task<Result<int>> HandleAsync(
        int projectId, 
        int boardId, 
        List<ReorderTicketCommand> ticketOrder, 
        CancellationToken cancellationToken)
    {
        // Validation 1
        if (ticketOrder.Count == 0)
        {
            return Result<int>.Fail("No tickets to reorder.");
        }
        
        // Validation 2
        var ticketIds = ticketOrder.Select(i => i.TicketId);
        var filter = new TicketSearchFilter
        {
            ProjectIds = [projectId], TicketIds = [.. ticketIds]
        };

        var foundTickets = await _ticketRepository.GetAllAsync(filter, cancellationToken);
        if (foundTickets.Count != ticketOrder.Count)
        {
            return Result<int>.Fail("One or more tickets were not found.");
        }

        // Validation 3
        var statusList = await _ticketRepository.GetTicketStatusListAsync(cancellationToken);
        var validStatusIds = new HashSet<int>(statusList.Select(s => s.Id));
        foreach (var cmd in ticketOrder.Where(c => c.StatusId.HasValue))
        {
            var statusId = cmd.StatusId!.Value;
            if (!validStatusIds.Contains(statusId))
            {
                return Result<int>.Fail($"TicketStatus with id {statusId} not found.");
            }
        }
        
        // Update
        var result = 
            await _ticketRepository.ReorderBoardTickets(
                projectId, 
                boardId == 0 ? null : boardId, 
                ticketOrder, 
                cancellationToken);

        return result;
    }
}