using FluentValidation;

namespace TaskSync.Domain.Ticket.CreateTicket;

public class CreateTicketCommandValidator : AbstractValidator<CreateTicketCommand>
{
    public CreateTicketCommandValidator()
    {
        RuleFor(x => x.Title).NotEmpty();
        RuleFor(x => x.ProjectId).NotEmpty();
        
        RuleForEach(x => x.Labels)
            .Must(label =>
                !string.IsNullOrWhiteSpace(label.Title) || label.LabelId.HasValue)
            .WithMessage("Each label must have either a Title or a LabelId.")
            .When(x => x.Labels != null && x.Labels.Count != 0);
    }
}