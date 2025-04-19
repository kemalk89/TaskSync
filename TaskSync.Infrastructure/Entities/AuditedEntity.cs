namespace TaskSync.Infrastructure.Entities;

public abstract class AuditedEntity
{
    public int CreatedBy { get; set; }
    public DateTimeOffset CreatedDate { get; set; }
    public DateTimeOffset? ModifiedDate { get; set; }
}
