using Deliscio.Modules.BuildingBlocks.Domain;
using MongoDB.Bson;

namespace Deliscio.Modules.Links.Domain.Events;

public sealed record LinkLikesCountChangedDomainEvent(
    ObjectId LinkId, 
    int OldValue, 
    int NewValue, 
    ObjectId UpdatedById) 
    : DomainEvent;