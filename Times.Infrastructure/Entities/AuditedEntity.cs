namespace Times.Infrastructure.Entities;

public abstract class AuditedEntity
{
    public DateTimeOffset CreatedDate { get; set; }
    public DateTimeOffset? ModifiedDate { get; set; }
}
