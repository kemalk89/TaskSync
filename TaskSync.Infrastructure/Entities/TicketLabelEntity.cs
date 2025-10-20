namespace TaskSync.Infrastructure.Entities;

/// <summary>
/// A label which can be attached to a ticket. Labels are scoped to Projects.
/// </summary>
public class TicketLabelEntity
{
    public int Id { get; set; }
    public string Text { get; set; } = string.Empty;
    public int ProjectId { get; set; }
}