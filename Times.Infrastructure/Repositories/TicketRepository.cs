using Times.Domain.Ticket;
using Times.Infrastructure.Entities;

namespace Times.Infrastructure.Repositories;

public class TicketRepository : ITicketRepository
{
    private readonly DatabaseContext _dbContext;

    public TicketRepository(DatabaseContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Ticket> CreateAsync(string title, string description)
    {
        var entity = new TicketEntity
        {
            Title = title,
            Description = description
        };

        await _dbContext.Tickets.AddAsync(entity);
        await _dbContext.SaveChangesAsync();

        return new Ticket
        {
            Id = entity.Id,
            Title = entity.Title,
            Description = entity.Description
        };
    }

    public Task<IEnumerable<Ticket>> GetAllAsync()
    {
        var tickets = _dbContext.Tickets
            .ToList()
            .Select(t => t.ToTicket());

        return Task.FromResult(tickets);
    }

    public async Task<Ticket?> GetByIdAsync(int id)
    {
        TicketEntity? entity = await _dbContext.Tickets.FindAsync(id);

        if (entity == null)
        {
            return null;
        }

        return new Ticket
        {
            Id = entity.Id,
            Title = entity.Title,
            Description = entity.Description
        };
    }
}
