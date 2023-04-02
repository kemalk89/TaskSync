namespace Times.Domain.Ticket.Command;

using Times.Domain.User;

public class CreateTicketCommand
{
    public int ProjectId { get; set; }

    public string Title { get; set; }

    public string? Description { get; set; }

    public User? Assignee { get; set; }
}
