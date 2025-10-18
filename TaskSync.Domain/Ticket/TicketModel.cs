namespace TaskSync.Domain.Ticket;

public class TicketModel
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }

    public int ProjectId { get; set; }
    public User.User? Assignee { get; set; }

    public List<TicketLabelModel> Labels { get; set; } = [];

    public User.User? CreatedBy { get; set; }

    public DateTimeOffset CreatedDate { get; set; }
    public DateTimeOffset? ModifiedDate { get; set; }

    public Project.Project Project { get; set; } = null!;
    public TicketStatus? Status { get; set; }
    public TicketType Type { get; set; }
}

public enum TicketType
{
    Bug,
    Task,
    Story
}