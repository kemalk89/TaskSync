using Microsoft.EntityFrameworkCore;
using Times.Domain.Project;
using Times.Domain.Shared;
using Times.Domain.Ticket;
using Times.Domain.Ticket.Command;
using Times.Infrastructure.Entities;

namespace Times.Infrastructure.Repositories;

public class TicketRepository : ITicketRepository
{
    private IUserRepository _userRepository;
    private readonly DatabaseContext _dbContext;
    private readonly IProjectRepository _projectRepository;

    public TicketRepository(DatabaseContext dbContext, IProjectRepository projectRepository, IUserRepository userRepository)
    {
        _dbContext = dbContext;
        _projectRepository = projectRepository;
        _userRepository = userRepository;
    }

    public async Task<Ticket?> CreateAsync(CreateTicketCommand cmd)
    {
        var project = _dbContext.Projects.Find(cmd.ProjectId);
        if (project != null)
        {
            var entity = new TicketEntity
            {
                Title = cmd.Title,
                Description = cmd.Description,
                Project = project
            };

            await _dbContext.Tickets.AddAsync(entity);
            await _dbContext.SaveChangesAsync();

            return new Ticket
            {
                Id = entity.Id,
                Title = entity.Title,
                Description = entity.Description,
                ProjectId = entity.Project.Id,
                Project = entity.Project.ToProject(),
            };
        }

        return null;
    }

    public Task<PagedResult<Ticket>> GetAllAsync(int pageNumber, int pageSize)
    {
        var skip = (pageNumber - 1) * pageSize;

        var tickets = _dbContext.Tickets
            .OrderBy(t => t.Title)
            .Include(t => t.Project)
            .Skip(skip)
            .Take(pageSize)
            .ToList()
            .Select(t => t.ToTicket());

        int total = _dbContext.Tickets.Count();

        var paged = new PagedResult<Ticket>
        {
            Items = tickets,
            PageNumber = pageNumber,
            PageSize = pageSize,
            Total = total
        };

        return Task.FromResult(paged);
    }

    public async Task<Ticket?> GetByIdAsync(int id)
    {
        TicketEntity? entity = await _dbContext.Tickets
            .Where(t => t.Id == id)
            .Include(t => t.Project)
            .FirstAsync();

        if (entity == null)
        {
            return null;
        }

        var createdBy = await _userRepository.FindUserByIdAsync(entity.CreatedBy);

        return entity.ToTicket(createdBy);
    }
}
