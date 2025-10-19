using FluentValidation;

namespace TaskSync.Domain.Ticket.Command;

public class CreateTicketCommandValidator : AbstractValidator<CreateTicketCommand>
{
    public CreateTicketCommandValidator()
    {
        RuleFor(x => x.Title).NotEmpty();
        RuleFor(x => x.ProjectId).NotEmpty();
    }
}