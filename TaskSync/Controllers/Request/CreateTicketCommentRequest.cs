using System.ComponentModel.DataAnnotations;
using TaskSync.Domain.Ticket.Command;

namespace TaskSync.Controllers.Request;

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
