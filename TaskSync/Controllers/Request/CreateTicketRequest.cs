using TaskSync.Domain.Ticket.Command;

namespace TaskSync.Controllers.Request;

public class CreateTicketRequest
{
    public int ProjectId { get; set; }

    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    public int? Assignee { get; set; }

    public string? Type { get; set; }
    
    public CreateTicketCommand ToCommand()
    {
        return new CreateTicketCommand
        {
            ProjectId = ProjectId,
            Description = Description,
            Title = Title,
            Assignee = Assignee,
            Type = Type
        };
    }
}

