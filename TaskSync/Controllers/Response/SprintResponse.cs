using TaskSync.Domain.Sprint;

namespace TaskSync.Controllers.Response;

public class SprintResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTimeOffset? StartDate { get; set; }
    public DateTimeOffset? EndDate { get; set; }
    public bool IsActive { get; set; }
    public List<TicketResponse>? Tickets { get; set; }

    // Parameterless constructor for integration tests (deserialization)
    public SprintResponse() { }

    public SprintResponse(SprintModel sprint)
    {
        Id = sprint.Id;
        Name = sprint.Name;
        StartDate = sprint.StartDate;
        EndDate = sprint.EndDate;
        IsActive = sprint.IsActive;
        Tickets = sprint.Tickets?.Select(t => new TicketResponse(t)).ToList();
    }
}
