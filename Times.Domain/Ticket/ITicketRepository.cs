using Times.Domain.Shared;
using Times.Domain.Ticket.Command;

namespace Times.Domain.Ticket;

public interface ITicketRepository
{
    Task<Ticket?> CreateAsync(CreateTicketCommand cmd);
    Task<Ticket?> GetByIdAsync(int id);
    Task<PagedResult<Ticket>> GetAllAsync(int pageNumber, int pageSize);
}
