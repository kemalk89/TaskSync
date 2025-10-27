using FluentValidation;

using TaskSync.Domain.Ticket.Command;

namespace TaskSync.Domain.Ticket.AddTicketComment;

public class AddTicketCommentCommandValidator : AbstractValidator<AddTicketCommentCommand>
{
    public AddTicketCommentCommandValidator()
    {
        RuleFor(x => x.Comment).NotEmpty();
    }
}