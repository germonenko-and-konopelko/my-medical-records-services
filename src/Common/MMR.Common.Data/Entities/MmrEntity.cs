namespace MMR.Common.Data.Entities;

public abstract class MmrEntity
{
    public long Id { get; set; }

    [Precision(0)]
    public DateTimeOffset CreatedAt { get; internal set; }

    [Precision(0)]
    public DateTimeOffset UpdatedAt { get; internal set; }
}