namespace Habanerio.Core.BuildingBlocks.Domain;

public class Entity<TType>
{
    private readonly List<IDomainEvent<TType>> _domainEvents = [];

    public virtual IEnumerable<IDomainEvent<TType>> DomainEvents => _domainEvents.AsEnumerable();

    public void AddDomainEvent(IDomainEvent<TType> eventItem)
    {
        _domainEvents.Add(eventItem);
    }

    public void RemoveDomainEvent(IDomainEvent<TType> eventItem)
    {
        _domainEvents.Remove(eventItem);
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}
