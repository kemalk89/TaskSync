using TaskSync.Domain.Shared;

namespace TaskSync.Domain.Ticket.QueryTicket;

public class QueryTicketCommandHandler : ICommandHandler
{
    private readonly ITicketRepository _ticketRepository;

    public QueryTicketCommandHandler(ITicketRepository ticketRepository)
    {
        _ticketRepository = ticketRepository;
    }


    public async Task<PagedResult<TicketCommentModel>> GetTicketCommentsAsync(int id, int pageNumber, int pageSize)
    {
        return await _ticketRepository.GetTicketCommentsAsync(id, pageNumber, pageSize);
    }
    
    public async Task<PagedResult<TicketModel>> GetTicketsByProjectIdAsync(int projectId, int pageNumber, int pageSize)
    {
        return await _ticketRepository.GetByProjectIdAsync(projectId, pageNumber, pageSize);
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
}