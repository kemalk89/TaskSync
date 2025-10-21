namespace TaskSync.Domain.Ticket.UpdateTicket;

public class UpdateTicketCommand
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public int? StatusId { get; set; }
}