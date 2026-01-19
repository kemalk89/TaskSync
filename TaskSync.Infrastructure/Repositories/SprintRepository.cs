using Microsoft.EntityFrameworkCore;

using TaskSync.Domain.Shared;
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

    public async Task<Result<SprintModel>> CreateAsync(AddSprintCommand command,CancellationToken cancellationToken)
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

        return Result<SprintModel>.Ok(entity.ToModel());
    }

    public async Task<Result<bool>> AssignTicketAsync(int sprintId, int ticketId, CancellationToken cancellationToken)
    {
        var ticket = await _dbContext.Tickets.FindAsync([ticketId], cancellationToken);
        if (ticket == null)
        {
            return Result<bool>.Fail(ResultCodes.ResultCodeResourceNotFound, "Ticket not found");
        }

        ticket.SprintId = sprintId;
        await _dbContext.SaveChangesAsync(cancellationToken);
        return Result<bool>.Ok(true);
    }

    public async Task<Result<SprintModel>> GetDraftSprintAsync(int projectId, CancellationToken cancellationToken)
    {
        var result = await _dbContext.Sprints
            .Where(s => s.ProjectId == projectId)
            .Where(s => !s.IsActive)
            .ToListAsync(cancellationToken);
        
        if (result.Count == 0)
        {
            return Result<SprintModel>.Fail(ResultCodes.ResultCodeResourceNotFound);
        }

        var sprint = result.FirstOrDefault();
        return Result<SprintModel>.Ok(sprint!.ToModel());
    }
}