using Times.Domain.Shared;
using Times.Domain.Ticket.Command;

namespace Times.Domain.Ticket;

public interface ITicketService
{
    Task<TicketModel?> CreateTicketAsync(CreateTicketCommand cmd);
    Task<TicketModel?> GetTicketByIdAsync(int id);
    Task<PagedResult<TicketModel>> GetTicketsAsync(int pageNumber, int pageSize);
    Task<PagedResult<TicketModel>> GetTicketsByProjectIdAsync(int projectId, int pageNumber, int pageSize);
    Task<TicketStatus> UpdateTicketStatusAsync(int ticketId, int statusId);
}
