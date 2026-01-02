namespace TaskSync.Domain.Project.ReorderBacklogTickets;

public class ReorderBacklogTicketCommand
{
    public int TicketId { get; set; }
    public int Position { get; set; }
}