using System.ComponentModel.DataAnnotations;

using TaskSync.Domain.Ticket.Command;

namespace TaskSync.Controllers.Request;

public record CreateTicketCommentRequest([Required] string Comment)
{
    public CreateTicketCommentCommand ToCommand() => new() { Comment = Comment };
}