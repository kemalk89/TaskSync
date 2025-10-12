using TaskSync.Domain.Ticket;
using TaskSync.Infrastructure.Entities;

namespace TaskSync.Infrastructure.Repositories;

public class LabelRepository : ILabelRepository
{
    private readonly DatabaseContext _dbContext;

    public LabelRepository(DatabaseContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<TicketLabelModel?> FindByIdAsync(int id)
    {
        var entity = await _dbContext.TicketLabels.FindAsync(id);
        return entity == null ? null : new TicketLabelModel { Id = entity.Id, Text = entity.Text };
    }

    public async Task<int> CreateLabelAsync(string text)
    {
        var entity = new TicketLabelEntity { Text = text };
        await _dbContext.TicketLabels.AddAsync(entity);
        await _dbContext.SaveChangesAsync();

        return entity.Id;
    }
}