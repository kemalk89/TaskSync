namespace Times.Domain.Ticket.Command;

public class CreateTicketCommand
{
    public int ProjectId { get; set; }

    public string Title { get; set; }

    public string? Description { get; set; }
}
