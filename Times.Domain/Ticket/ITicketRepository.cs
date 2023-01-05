namespace Times.Domain.Ticket;

public interface ITicketRepository
{
    Task<Ticket> CreateAsync(string title, string description);
    Task<Ticket?> GetByIdAsync(int id);
    Task<IEnumerable<Ticket>> GetAllAsync();
}
