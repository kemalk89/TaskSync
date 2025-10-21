namespace TaskSync.Domain.Ticket.AssignTicketLabel;

public class AssignTicketLabelCommand
{
    public int? LabelId { get; set; }
    public string? Title { get; set; }
}