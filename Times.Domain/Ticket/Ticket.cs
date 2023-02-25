namespace Times.Domain.Ticket;

using Times.Domain.Project;

public class Ticket
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }

    public int ProjectId { get; set; }
    public DateTimeOffset CreatedDate { get; set; }
    public DateTimeOffset? ModifiedDate { get; set; }

    public Project Project { get; set; }

}
