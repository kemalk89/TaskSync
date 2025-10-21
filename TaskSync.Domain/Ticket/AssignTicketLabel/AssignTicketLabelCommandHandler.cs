using FluentValidation;

using TaskSync.Domain.Exceptions;
using TaskSync.Domain.Project;
using TaskSync.Domain.Shared;

namespace TaskSync.Domain.Ticket.AssignTicketLabel;

public class AssignTicketLabelCommandHandler : ICommandHandler
{
    private readonly IValidator<AssignTicketLabelCommand> _assignTicketLabelCommandValidator;
    private readonly ITicketRepository _ticketRepository;
    private readonly IProjectRepository _projectRepository;

    public AssignTicketLabelCommandHandler(
        ITicketRepository ticketRepository, 
        IValidator<AssignTicketLabelCommand> assignTicketLabelCommandValidator, 
        IProjectRepository projectRepository)
    {
        _ticketRepository = ticketRepository;
        _assignTicketLabelCommandValidator = assignTicketLabelCommandValidator;
        _projectRepository = projectRepository;
    }

    public async Task<Result<int>> HandleAsync(int ticketId, AssignTicketLabelCommand cmd)
    {
        // Validation
        var validationResult = await _assignTicketLabelCommandValidator.ValidateAsync(cmd);
        if (!validationResult.IsValid)
        {
            return Result<int>.Fail(ResultCodes.ResultCodeValidationFailed, validationResult);
        }
        
        var ticket = await _ticketRepository.GetByIdAsync(ticketId);
        if (ticket is null)
        {
            return Result<int>.Fail(ResultCodes.ResultCodeResourceNotFound, ErrorDetails.TicketNotFound);
        }
        
        var labelId = cmd.LabelId;
        if (labelId != null)
        {
            var alreadyAssigned = ticket.Labels.Find(i => i.Id == labelId) != null;
            if (alreadyAssigned)
            {
                return Result<int>.Fail(
                    ResultCodes.ResultCodeValidationFailed,
                    $"Label ID {labelId} already assigned to ticket.");
            }             
        }
        
        // Create new label if needed for the project
        if (labelId == null && !string.IsNullOrWhiteSpace(cmd.Title))
        {
            labelId = await _projectRepository.CreateLabelAsync(ticket.ProjectId, cmd.Title);
        }

        if (labelId == null)
        {
            throw new DomainException("Unknown Domain Exception.");
        }
        
        // Assign label to ticket
        var assignLabelResult = await _ticketRepository.AssignTicketLabelAsync(ticket.ProjectId, ticketId, labelId.Value);
        return assignLabelResult;
    }
}