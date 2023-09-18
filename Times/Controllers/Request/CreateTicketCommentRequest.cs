using System.ComponentModel.DataAnnotations;
using Times.Domain.Ticket.Command;

namespace Times.Controllers.Request;

public class CreateTicketCommentRequest
{   
    [Required]
    public string Comment { get; set; }
    
    public CreateTicketCommentCommand ToCommand()
    {
        return new CreateTicketCommentCommand()
        {
            Comment = Comment
        };
    }
}
