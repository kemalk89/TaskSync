using Times.Domain.Project;
using Times.Domain.Ticket;
using Times.Domain.Ticket.Command;
using Times.Infrastructure.Entities;

namespace Times.Infrastructure.Repositories;

public class TicketRepository : ITicketRepository
{
    private readonly DatabaseContext _dbContext;
    private readonly IProjectRepository _projectRepository;

    public TicketRepository(DatabaseContext dbContext, IProjectRepository projectRepository)
    {
        _dbContext = dbContext;
        _projectRepository = projectRepository;
    }

    public async Task<Ticket?> CreateAsync(CreateTicketCommand cmd)
    {
        var entity = new TicketEntity
        {
            Title = cmd.Title,
            Description = cmd.Description,
            ProjectId = cmd.ProjectId
        };

        await _dbContext.Tickets.AddAsync(entity);
        await _dbContext.SaveChangesAsync();

        return new Ticket
        {
            Id = entity.Id,
            Title = entity.Title,
            Description = entity.Description,
            ProjectId = entity.Project.Id
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

        return entity.ToTicket();
    }
}
