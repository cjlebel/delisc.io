namespace Habanerio.Core.BuildingBlocks.Domain;

public record DomainEventBase<TId> : IDomainEvent<TId>
{
    public TId Id { get; }

    public DateTimeOffset DateOccurred { get; } = DateTime.UtcNow;

    protected DomainEventBase(TId id)
    {
        Id = id;
        DateOccurred = DateTimeOffset.UtcNow;
    }
}