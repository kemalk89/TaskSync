using TaskSync.Domain.Ticket;

namespace TaskSync.Infrastructure.Entities;

/// <summary>
/// Represents status of a ticket. The status of tickets is global and not scoped to a project.
/// </summary>
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
