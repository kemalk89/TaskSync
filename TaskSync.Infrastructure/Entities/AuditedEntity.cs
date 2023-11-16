namespace TaskSync.Infrastructure.Entities;

public abstract class AuditedEntity
{
    public string CreatedBy { get; set; }
    public DateTimeOffset CreatedDate { get; set; }
    public DateTimeOffset? ModifiedDate { get; set; }
}
