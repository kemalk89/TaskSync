namespace TaskSync.Domain.Project.ReorderBacklogTickets;

public class ReorderTicketCommand
{
    public int TicketId { get; set; }
    public int Position { get; set; }
}