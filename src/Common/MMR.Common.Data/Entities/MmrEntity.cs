namespace MMR.Common.Data.Entities;

public abstract class MmrEntity
{
    public long Id { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset UpdatedAt { get; set; }
}