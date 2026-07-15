using Microsoft.EntityFrameworkCore;

using TaskSync.Domain.Shared;
using TaskSync.Domain.Sprint;
using TaskSync.Domain.Sprint.AddSprint;
using TaskSync.Infrastructure.Entities;

namespace TaskSync.Infrastructure.Repositories;

public class SprintRepository : ISprintRepository
{
    private readonly DatabaseContext _dbContext;

    public SprintRepository(DatabaseContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<SprintModel>> CreateAsync(AddSprintCommand command, CancellationToken cancellationToken)
    {
        var entity = new SprintEntity
        {
            Name = command.Name,
            StartDate = command.StartDate,
            EndDate = command.EndDate,
            ProjectId = command.ProjectId
        };

        await _dbContext.Sprints.AddAsync(entity, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        if (command.TicketIds.Count > 0)
        {
            var tickets = await _dbContext.Tickets
                .Where(t => command.TicketIds.Contains(t.Id) && t.ProjectId == command.ProjectId)
                .ToListAsync(cancellationToken);

            foreach (var ticket in tickets)
            {
                ticket.SprintId = entity.Id;
            }

            await _dbContext.SaveChangesAsync(cancellationToken);
        }

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

    public async Task<Result<bool>> HasRunningSprintAsync(int projectId, DateTimeOffset startDate, CancellationToken cancellationToken)
    {
        var exists = await _dbContext.Sprints
            .AnyAsync(s => s.ProjectId == projectId &&
                           s.StartDate != null &&
                           startDate <= s.EndDate, cancellationToken);
        return Result<bool>.Ok(exists);
    }

    public async Task<Result<SprintModel>> GetActiveSprintAsync(int projectId, CancellationToken cancellationToken)
    {
        var now = DateTimeOffset.UtcNow;
        var sprint = await _dbContext.Sprints
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.ProjectId == projectId
                                      && s.StartDate <= now
                                      && s.EndDate >= now, cancellationToken);

        if (sprint == null)
        {
            return Result<SprintModel>.Fail(ResultCodes.ResultCodeResourceNotFound);
        }

        return Result<SprintModel>.Ok(sprint.ToModel());
    }

    public async Task<Result<SprintModel>> GetDraftSprintAsync(int projectId, CancellationToken cancellationToken)
    {
        var result = await _dbContext.Sprints
            .Where(s => s.ProjectId == projectId)
            .Where(s => s.StartDate == null && s.EndDate == null)
            .ToListAsync(cancellationToken);

        if (result.Count == 0)
        {
            return Result<SprintModel>.Fail(ResultCodes.ResultCodeResourceNotFound);
        }

        var sprint = result.FirstOrDefault();
        return Result<SprintModel>.Ok(sprint!.ToModel());
    }

    public async Task<PagedResult<SprintModel>> GetSprintsAsync(int projectId, PaginationQuery paginationQuery, CancellationToken cancellationToken)
    {
        var query = _dbContext.Sprints
            .Where(s => s.ProjectId == projectId)
            .Where(s => s.StartDate != null);

        var total = await query.CountAsync(cancellationToken);

        var pageNumber = paginationQuery.PageNumber;
        var pageSize = paginationQuery.PageSize;
        var skip = (pageNumber - 1) * pageSize;

        var sprints = await query
            .Skip(skip)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<SprintModel>
        {
            PageNumber = pageNumber,
            PageSize = pageSize,
            Total = total,
            Items = sprints.Select(s => s.ToModel())
        };
    }
}