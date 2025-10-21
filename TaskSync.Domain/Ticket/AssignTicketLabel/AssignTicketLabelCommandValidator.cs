using FluentValidation;

namespace TaskSync.Domain.Ticket.AssignTicketLabel;

public class AssignTicketLabelCommandValidator :  AbstractValidator<AssignTicketLabelCommand>
{
    public AssignTicketLabelCommandValidator()
    {
        RuleFor(x => x)
            .Must(cmd => cmd.LabelId.HasValue || !string.IsNullOrWhiteSpace(cmd.Title))
            .WithMessage(cmd =>
                $"Both fields {nameof(cmd.LabelId)} and {nameof(cmd.Title)} cannot be null or empty.");
    }
}