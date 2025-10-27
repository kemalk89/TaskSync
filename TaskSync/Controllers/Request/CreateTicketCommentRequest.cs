using TaskSync.Domain.Ticket.Command;

namespace TaskSync.Controllers.Request;

public record CreateTicketCommentRequest(string Comment)
{
    public AddTicketCommentCommand ToCommand() => new() { Comment = Comment };
}