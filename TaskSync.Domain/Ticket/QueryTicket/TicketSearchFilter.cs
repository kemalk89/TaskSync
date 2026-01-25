using System.Linq.Expressions;

namespace TaskSync.Domain.Ticket.QueryTicket;

public class TicketSearchFilter
{
    public string SearchText { get; set; } = string.Empty;
    public List<int> StatusIds { get; set; } = [];
    public List<int> ProjectIds { get; set; } = [];
    public List<int> AssigneeIds { get; set; } = [];
    public List<int> TicketIds { get; set; } = [];
    public int BoardId { get; set; }
    public bool OnlyBacklogTickets { get; set; }
    public string? OrderBy { get; set; }
}