namespace Times.Domain.Ticket;

using Times.Domain.Project;
using Times.Domain.User;

public class TicketModel
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public string? Description { get; set; }

    public int ProjectId { get; set; }
    public User? Assignee { get; set; }

    public User? CreatedBy { get; set; }

    public DateTimeOffset CreatedDate { get; set; }
    public DateTimeOffset? ModifiedDate { get; set; }

    public Project Project { get; set; } = null!;
    public TicketStatus? Status { get; set; }

}
