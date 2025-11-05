using System.Text.Json.Serialization;

using TaskSync.Controllers.Project;
using TaskSync.Domain.Ticket;
using TaskSync.Domain.User;

namespace TaskSync.Controllers.Response;

public class TicketResponse
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public User? Assignee { get; set; }
    public User? CreatedBy { get; set; }
    public DateTimeOffset CreatedDate { get; set; } 
    public DateTimeOffset? ModifiedDate { get; set; }
    [JsonPropertyName("Status")]
    public TicketStatusResponse? Status { get; set; }
    public ProjectResponse Project { get; set; } = null!;
    public string? Type { get; set; }
    public List<TicketLabelModel>? Labels { get; set; }

    public TicketResponse()
    {
        // Parameterless constructor used for deserialization in tests
    }

    public TicketResponse(TicketModel ticket)
    {
        Id = ticket.Id;
        Title = ticket.Title;
        Description = ticket.Description;
        Assignee = ticket.Assignee;
        CreatedBy = ticket.CreatedBy;
        CreatedDate = ticket.CreatedDate;
        ModifiedDate = ticket.ModifiedDate;
        Status = ticket.Status != null ? new TicketStatusResponse(ticket.Status) : null;
        Project = new ProjectResponse(ticket.ProjectModel);
        Labels = ticket.Labels;
        Type = ticket.Type switch
        {
            TicketType.Task => "task",
            TicketType.Bug => "bug",
            TicketType.Story => "story",
            _ => Type
        };
    }
}
