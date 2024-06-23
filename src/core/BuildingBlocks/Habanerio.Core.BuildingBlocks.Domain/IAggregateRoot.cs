namespace Habanerio.Core.BuildingBlocks.Domain;

public interface IAggregateRoot<TId, TType> where TId : TypedIdValueBase<TType>
{
    TId Id { get; }
}
