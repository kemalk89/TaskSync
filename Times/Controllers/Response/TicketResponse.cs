using Times.Domain.Shared;
using Times.Domain.Ticket;

namespace Times.Controllers.Response;

public class TicketResponse
{

    public int Id { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public User? CreatedBy { get; set; }
    public DateTimeOffset CreatedDate { get; set; }
    public DateTimeOffset? ModifiedDate { get; set; }
    public ProjectResponse Project { get; set; }

    public TicketResponse(Ticket ticket)
    {
        Id = ticket.Id;
        Title = ticket.Title;
        Description = ticket.Description;
        CreatedBy = ticket.CreatedBy;
        CreatedDate = ticket.CreatedDate;
        ModifiedDate = ticket.ModifiedDate;
        Project = new ProjectResponse(ticket.Project);
    }
}
