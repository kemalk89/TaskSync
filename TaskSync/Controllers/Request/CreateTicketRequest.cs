using System.ComponentModel.DataAnnotations;

using TaskSync.Domain.Ticket.Command;

namespace TaskSync.Controllers.Request;

public class CreateTicketRequest
{
    [Range(1, int.MaxValue, ErrorMessage = "Only positive numbers allowed.")]
    public int ProjectId { get; set; }

    [Required]
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

