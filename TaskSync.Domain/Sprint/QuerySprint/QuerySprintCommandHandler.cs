using FluentValidation;

using TaskSync.Domain.Shared;
using TaskSync.Domain.Ticket;
using TaskSync.Domain.Ticket.QueryTicket;

namespace TaskSync.Domain.Sprint.QuerySprint;

public class QuerySprintCommandHandler : ICommandHandler
{
    private readonly ISprintRepository _sprintRepository;
    private readonly ITicketRepository _ticketRepository;
    private readonly IValidator<PaginationQuery> _paginationQueryValidator;

    public QuerySprintCommandHandler(
        ISprintRepository sprintRepository,
        ITicketRepository ticketRepository,
        IValidator<PaginationQuery> paginationQueryValidator)
    {
        _sprintRepository = sprintRepository;
        _ticketRepository = ticketRepository;
        _paginationQueryValidator = paginationQueryValidator;
    }

    public async Task<Result<SprintModel>> GetDraftSprintAsync(int projectId, CancellationToken cancellationToken)
    {
        var sprint = await _sprintRepository.GetDraftSprintAsync(projectId, cancellationToken);
        if (!sprint.Success)
        {
            return sprint;
        }

        var tickets = await _ticketRepository.GetAllAsync(new TicketSearchFilter
        {
            BoardId = sprint.Value!.Id,
            OrderBy = TicketModel.OrderByPosition,
        }, cancellationToken);

        sprint.Value.Tickets = tickets;

        return sprint;
    }

    public async Task<Result<PagedResult<SprintModel>>> GetProjectSprintsAsync(int projectId, PaginationQuery paginationQuery, CancellationToken cancellationToken)
    {
        var validationResult = _paginationQueryValidator.Validate(paginationQuery);
        if (validationResult.IsValid)
        {
            var pagedResult = await _sprintRepository.GetSprintsAsync(projectId, paginationQuery, cancellationToken);
            return Result<PagedResult<SprintModel>>.Ok(pagedResult);
        }

        return Result<PagedResult<SprintModel>>.Fail(ResultCodes.ResultCodeValidationFailed, validationResult);
    }
}