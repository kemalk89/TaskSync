using TaskSync.Domain.Shared;
using TaskSync.Domain.Ticket.Command;

namespace TaskSync.Domain.Ticket;

public interface ITicketRepository
{
    Task<int?> CreateAsync(CreateTicketCommand cmd);
    Task<TicketModel?> GetByIdAsync(int id);
    Task<PagedResult<TicketModel>> GetByProjectIdAsync(int projectId, int pageNumber, int pageSize);
    Task<PagedResult<TicketModel>> GetAllAsync(int pageNumber, int pageSize, TicketSearchFilter filter);
    Task<Result<bool>> UpdateTicketAsync(int ticketId, UpdateTicketCommand updateTicketCommand);
    Task<TicketCommentModel> AddTicketCommentAsync(int ticketId, CreateTicketCommentCommand cmd);
    Task<TicketCommentModel?> GetTicketCommentByIdAsync(int commentId);
    Task<PagedResult<TicketCommentModel>> GetTicketCommentsAsync(int id, int pageNumber, int pageSize);
    Task<int> DeleteTicketAsync(int id);
    Task<bool> DeleteTicketCommentAsync(int commentId);
    Task<Result<bool>> AssignTicketLabelAsync(int ticketId, int labelId);
}
