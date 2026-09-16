using FluentValidation;

namespace TaskSync.Domain.Ticket.CreateTicket;

public class CreateTicketCommandValidator : AbstractValidator<CreateTicketCommand>
{
    public CreateTicketCommandValidator(ITicketRepository ticketRepository)
    {
        RuleFor(x => x.Title).NotEmpty();
        RuleFor(x => x.ProjectId).NotEmpty();
        
        RuleFor(x => x.ParentId)
            .GreaterThan(0)
            .When(x => x.ParentId.HasValue);

        RuleFor(x => x.ParentId)
            .MustAsync(async (parentId, cancellation) =>
            {
                var parentTicket = await ticketRepository.GetByIdAsync(parentId!.Value);
                return parentTicket != null;
            })
            .WithMessage("Parent ticket with id {PropertyValue} not exists")
            .When(x => x.ParentId.HasValue);
        
        RuleForEach(x => x.Labels)
            .Must(label =>
                !string.IsNullOrWhiteSpace(label.Title) || label.LabelId.HasValue)
            .WithMessage("Each label must have either a Title or a LabelId.")
            .When(x => x.Labels != null && x.Labels.Count != 0);
    }
}