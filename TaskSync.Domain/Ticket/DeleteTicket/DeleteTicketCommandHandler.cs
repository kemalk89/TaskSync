using TaskSync.Domain.Shared;
using TaskSync.Domain.User;

namespace TaskSync.Domain.Ticket.DeleteTicket;

public class DeleteTicketCommandHandler : ICommandHandler
{
    private readonly ITicketRepository _ticketRepository;
    private readonly ICurrentUserService _currentUserService;

    public DeleteTicketCommandHandler(ICurrentUserService currentUserService, ITicketRepository ticketRepository)
    {
        _currentUserService = currentUserService;
        _ticketRepository = ticketRepository;
    }


    public async Task<Result<bool>> HandleAsync(int id)
    {
        // Check if the current user is an admin, or part of the project team or is author of the ticket
        var ticket = await _ticketRepository.GetByIdAsync(id);
        if (ticket is null)
        {
            return Result<bool>.Fail("No ticket found with ID " + id);
        }
        
        // TODO if the current user has role ADMIN, allow deletion
        
        // TODO if the current user is part of the project team, allow deletion
        // TODO var project = ticket.Project;

        // if the current user is author of the ticket, allow deletion
        var author = ticket.CreatedBy;
        if (author != null)
        {
            var currentUser = await _currentUserService.GetCurrentUserAsync();
            if (currentUser?.Id == author.Id)
            {
                await _ticketRepository.DeleteTicketAsync(id);
                return Result<bool>.Ok(true);
            }
        }

        return Result<bool>.Fail("Could not delete the ticket with ID " + id);
    }
}