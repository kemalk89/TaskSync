using TaskSync.Domain.Ticket.AssignTicketLabel;

namespace TaskSync.Domain.Ticket.CreateTicket;

public class CreateTicketCommand
{
    public int ProjectId { get; set; }

    public string Title { get; set; } = string.Empty;
 
    public string? Description { get; set; }

    public int? Assignee { get; set; }
    
    public int? ParentId { get; set; }
    
    public string? Type { get; set; }
    
    public List<AssignTicketLabelCommand>? Labels { get; set; }
    
    public int? StatusId { get; set; }
}
