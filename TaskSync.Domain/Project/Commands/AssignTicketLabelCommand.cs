namespace TaskSync.Domain.Project.Commands;

public class AssignTicketLabelCommand
{
    public int? LabelId { get; set; }
    public string? Title { get; set; }
}