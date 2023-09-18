using Times.Domain.Shared;
using Times.Domain.Ticket.Command;

namespace Times.Domain.Ticket;

public interface ITicketRepository
{
    Task<TicketModel?> CreateAsync(CreateTicketCommand cmd);
    Task<TicketModel?> GetByIdAsync(int id);
    Task<PagedResult<TicketModel>> GetByProjectIdAsync(int projectId, int pageNumber, int pageSize);
    Task<PagedResult<TicketModel>> GetAllAsync(int pageNumber, int pageSize);
    Task<TicketStatus> UpdateTicketStatusAsync(int ticketId, int statusId);
    Task<TicketCommentModel> AddTicketCommentAsync(int ticketId, CreateTicketCommentCommand cmd);
    Task<PagedResult<TicketCommentModel>> GetTicketCommentsAsync(int id, int pageNumber, int pageSize);
}
