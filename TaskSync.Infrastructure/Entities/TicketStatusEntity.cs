using TaskSync.Domain.Ticket;

namespace TaskSync.Infrastructure.Entities;

public class TicketStatusEntity
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public TicketStatus ToDomainObject()
    {
        return new TicketStatus
        {
            Id = Id,
            Name = Name
        };
    }
}
