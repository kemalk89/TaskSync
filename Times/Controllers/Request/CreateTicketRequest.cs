using System.ComponentModel.DataAnnotations;
using Times.Domain.Ticket.Command;

namespace Times.Controllers.Request;

public class CreateTicketRequest
{
    [Range(1, int.MaxValue, ErrorMessage = "Only positive numbers allowed.")]
    public int ProjectId { get; set; }

    [Required]
    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public UserRequest? Assignee { get; set; }

    public CreateTicketCommand ToCommand()
    {
        return new CreateTicketCommand
        {
            ProjectId = ProjectId,
            Description = Description,
            Title = Title,
            Assignee = Assignee?.ToUser()
        };
    }
}
