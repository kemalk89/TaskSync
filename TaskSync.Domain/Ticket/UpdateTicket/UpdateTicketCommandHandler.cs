using TaskSync.Domain.Shared;

namespace TaskSync.Domain.Ticket.UpdateTicket;

public class UpdateTicketCommandHandler : ICommandHandler
{
    private readonly ITicketRepository _ticketRepository;

    public UpdateTicketCommandHandler(ITicketRepository ticketRepository)
    {
        _ticketRepository = ticketRepository;
    }

    public async Task<Result<bool>> HandleAsync(int ticketId, UpdateTicketCommand updateTicketCommand)
    {   
        var result = await _ticketRepository.UpdateTicketAsync(ticketId, updateTicketCommand);
        return result;
    }
    
}