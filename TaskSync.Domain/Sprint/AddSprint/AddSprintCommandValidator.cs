using FluentValidation;

using TaskSync.Domain.Project;

namespace TaskSync.Domain.Sprint.AddSprint;

public class AddSprintCommandValidator : AbstractValidator<AddSprintCommand>
{
    public AddSprintCommandValidator(IProjectRepository projectRepository)
    {

        RuleFor(x => x.EndDate).NotNull();
        RuleFor(x => x.ProjectId).NotNull().GreaterThan(0);
        RuleFor(x => x.EndDate)
            .GreaterThan(DateTimeOffset.Now)
            .WithMessage("The Sprint EndDate cannot be in the past");

        RuleFor(x => x.ProjectId).MustAsync(async (id, cancellation) =>
        {
            var exists = await projectRepository.GetByIdAsync(id);
            return exists != null;
        }).WithMessage("Project with id {PropertyValue} not exists");
    }
}