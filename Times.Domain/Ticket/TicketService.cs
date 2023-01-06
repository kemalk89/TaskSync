namespace Times.Domain.Ticket;

public class TicketService : ITicketService
{
    private readonly ITicketRepository _ticketRepository;

    public TicketService(ITicketRepository taskRepository)
    {
        _ticketRepository = taskRepository;
    }

    public async Task<Ticket> CreateTicketAsync(string title, string? description)
    {
        var ticket = await _ticketRepository.CreateAsync(title, description);
        return ticket;
    }

    public async Task<Ticket?> GetTicketByIdAsync(int id)
    {
        var ticket = await _ticketRepository.GetByIdAsync(id);
        // TODO what if not found?
        return ticket;
    }

    public async Task<IEnumerable<Ticket>> GetTicketsAsync()
    {
        var tickets = await _ticketRepository.GetAllAsync();
        return tickets;
    }
}
