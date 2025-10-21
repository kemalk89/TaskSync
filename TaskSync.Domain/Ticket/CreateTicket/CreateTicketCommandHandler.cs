using FluentValidation;

using TaskSync.Domain.Exceptions;
using TaskSync.Domain.Project;
using TaskSync.Domain.Shared;
using TaskSync.Domain.Ticket.AssignTicketLabel;

namespace TaskSync.Domain.Ticket.CreateTicket;

public class CreateTicketCommandHandler : ICommandHandler
{
    private readonly ITicketRepository _ticketRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly IValidator<CreateTicketCommand> _createTicketCommandValidator;
    private readonly AssignTicketLabelCommandHandler _assignTicketLabelCommandHandler;
    
    public CreateTicketCommandHandler(
        IValidator<CreateTicketCommand> createTicketCommandValidator, 
        IProjectRepository projectRepository, 
        ITicketRepository ticketRepository,
        AssignTicketLabelCommandHandler assignTicketLabelCommandHandler)
    {
        _createTicketCommandValidator = createTicketCommandValidator;
        _projectRepository = projectRepository;
        _ticketRepository = ticketRepository;
        _assignTicketLabelCommandHandler = assignTicketLabelCommandHandler;
    }

    public async Task<Result<int>> HandleAsync(CreateTicketCommand cmd)
    {
        var result = await _createTicketCommandValidator.ValidateAsync(cmd);
        if (!result.IsValid)
        {
            return Result<int>.Fail(ResultCodes.ResultCodeValidationFailed, result);
        }
        
        var project = await _projectRepository.GetByIdAsync(cmd.ProjectId);
        if (project == null)
        {
            return Result<int>.Fail(ResultCodes.ResultCodeResourceNotFound, result);
        }
        
        // Create the new ticket
        var ticketId = await _ticketRepository.CreateAsync(cmd);
        if (ticketId == null)
        {
            throw new DomainException("Could not create ticket");
        }
        
        // Assign Labels
        if (cmd.Labels?.Count > 0)
        {
            foreach (var newLabel in cmd.Labels)
            {
                var assignTicketLabelResult = await _assignTicketLabelCommandHandler.HandleAsync(
                    ticketId.Value, newLabel);
                if (!assignTicketLabelResult.Success)
                {
                    return assignTicketLabelResult;
                }
            }
            
        }
        
        return Result<int>.Ok(ticketId.Value);
    }
}
