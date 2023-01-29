using System.ComponentModel.DataAnnotations;
using Times.Domain.Ticket.Command;

namespace Times.Controllers.Request;

public class CreateTicketRequest
{
    public int ProjectId { get; set; }

    public string Title { get; set; }

    public string? Description { get; set; }


    public CreateTicketCommand ToCommand()
    {
        return new CreateTicketCommand
        {
            ProjectId = ProjectId,
            Description = Description,
            Title = Title
        };
    }

}
