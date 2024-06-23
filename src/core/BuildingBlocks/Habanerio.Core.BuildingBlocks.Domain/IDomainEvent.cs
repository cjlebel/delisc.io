using MediatR;

namespace Habanerio.Core.BuildingBlocks.Domain;
public interface IDomainEvent<out TId> : INotification
{
    TId Id { get; }

    DateTimeOffset DateOccurred { get; }
}