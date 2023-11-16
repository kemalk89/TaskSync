using TaskSync.Domain.Ticket;

namespace TaskSync.Controllers.Response;

public class TicketStatusResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;

    public TicketStatusResponse(TicketStatus status)
    {
        Id = status.Id;
        Name = status.Name;
    }
}
