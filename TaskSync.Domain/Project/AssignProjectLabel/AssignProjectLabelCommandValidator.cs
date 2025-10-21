using FluentValidation;

namespace TaskSync.Domain.Project.AssignProjectLabel;

public class AssignProjectLabelCommandValidator :  AbstractValidator<AssignProjectLabelCommand>
{
    public AssignProjectLabelCommandValidator()
    {
        RuleFor(x => x.ProjectId).NotEmpty();
        RuleFor(x => x.Text).NotEmpty();
    }
}