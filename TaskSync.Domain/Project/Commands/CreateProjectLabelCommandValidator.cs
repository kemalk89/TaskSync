using FluentValidation;

namespace TaskSync.Domain.Project.Commands;

public class CreateProjectLabelCommandValidator :  AbstractValidator<CreateProjectLabelCommand>
{
    public CreateProjectLabelCommandValidator()
    {
        RuleFor(x => x.ProjectId).NotEmpty();
        RuleFor(x => x.Text).NotEmpty();
    }
}