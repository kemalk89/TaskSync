using Times.Domain.Ticket.Command;

namespace Times.Domain.Ticket;

public interface ITicketService
{
    Task<Ticket> CreateTicketAsync(CreateTicketCommand cmd);
    Task<Ticket?> GetTicketByIdAsync(int id);
    Task<IEnumerable<Ticket>> GetTicketsAsync();
}
