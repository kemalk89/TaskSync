using TaskSync.Domain.Project;
using TaskSync.Domain.Shared;
using TaskSync.Domain.Sprint.AddSprint;

namespace TaskSync.Domain.Sprint.AssignTicket;

public class AssignTicketToSprintCommandHandler : ICommandHandler
{
    private readonly ISprintRepository _sprintRepository;
    private readonly IProjectRepository _projectRepository;

    public AssignTicketToSprintCommandHandler(ISprintRepository sprintRepository, IProjectRepository projectRepository)
    {
        _sprintRepository = sprintRepository;
        _projectRepository = projectRepository;
    }

    public async Task<Result<bool>> HandleAsync(int projectId, int sprintId, int ticketId, CancellationToken cancellationToken)
    {
        // validation: Does project exist?
        var project = await _projectRepository.GetByIdAsync(projectId);
        if (project == null)
        {
            return Result<bool>.Fail(ResultCodes.ResultCodeResourceNotFound, "Project not found");
        }
        
        // If sprintId == 0, we are going to assign ticket to draft sprint
        if (sprintId == 0)
        {
            // Validation: Is draft sprint already exists?
            var draftSprint = await _sprintRepository.GetDraftSprintAsync(projectId, cancellationToken);
            if (!draftSprint.Success)
            {
                // Create new draft sprint on the fly
                draftSprint = await _sprintRepository.CreateAsync(new AddSprintCommand
                {
                    ProjectId = projectId,
                    IsActive = false
                }, cancellationToken);
                
                if (!draftSprint.Success)
                {
                    return Result<bool>.Fail(ResultCodes.ResultCodeResourceNotFound, "No sprint in draft mode found");
                }
            }
            
            sprintId = draftSprint.Value!.Id;
        }
        
        var result = await _sprintRepository.AssignTicketAsync(sprintId, ticketId, cancellationToken);
        return result;
    }
}