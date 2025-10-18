namespace TaskSync.Domain.Ticket.Command;

public class CreateTicketCommand
{
    public int ProjectId { get; set; }

    public string Title { get; set; } = string.Empty;
 
    public string? Description { get; set; }

    public int? Assignee { get; set; }
    
    public string? Type { get; set; }
}
