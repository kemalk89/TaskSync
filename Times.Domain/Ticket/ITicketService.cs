namespace Times.Domain.Ticket;

public interface ITicketService
{
    Task<Ticket> CreateTicketAsync(string title, string? description);
    Task<Ticket?> GetTicketByIdAsync(int id);
    Task<IEnumerable<Ticket>> GetTicketsAsync();
}
