using FluentValidation;

using TaskSync.Domain.Project;

namespace TaskSync.Domain.Sprint.AddSprint;

public class AddSprintCommandValidator : AbstractValidator<AddSprintCommand>
{
    public AddSprintCommandValidator(IProjectRepository projectRepository, ISprintRepository sprintRepository)
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

        RuleFor(x => x.ProjectId).MustAsync(async (id, cancellation) =>
        {
            var conflictCheck = await sprintRepository.HasRunningSprintAsync(id, DateTimeOffset.UtcNow, cancellation);
            return !conflictCheck.Value;
        }).WithMessage("A sprint is already running for this project {PropertyValue}");
    }
}