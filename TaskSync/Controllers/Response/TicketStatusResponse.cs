using TaskSync.Domain.Ticket;

namespace TaskSync.Controllers.Response;

public class TicketStatusResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public TicketStatusResponse()
    {
        // Parameterless constructor used for deserialization in tests
    }

    public TicketStatusResponse(TicketStatusModel statusModel)
    {
        Id = statusModel.Id;
        Name = statusModel.Name;
    }
}
