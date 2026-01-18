using TaskSync.Domain.Sprint;
using TaskSync.Domain.Sprint.AddSprint;
using TaskSync.Infrastructure.Entities;

namespace TaskSync.Infrastructure.Repositories;

public class SprintRepository: ISprintRepository
{
    private readonly DatabaseContext _dbContext;

    public SprintRepository(DatabaseContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<SprintModel> CreateAsync(AddSprintCommand command,CancellationToken cancellationToken)
    {
        var entity = new SprintEntity
        {
            Name = command.Name,
            StartDate = command.StartDate,
            EndDate = command.EndDate,
            IsActive = command.IsActive,
            ProjectId =  command.ProjectId
        };

        await _dbContext.Sprints.AddAsync(entity, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return entity.ToModel();
    }
}