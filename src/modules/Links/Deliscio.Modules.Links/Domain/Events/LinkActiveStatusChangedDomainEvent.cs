using Deliscio.Modules.BuildingBlocks.Domain;
using MongoDB.Bson;

namespace Deliscio.Modules.Links.Domain.Events;

public sealed record LinkActiveStatusChangedDomainEvent(
    ObjectId LinkId, 
    bool NewStatus, 
    ObjectId UpdatedById) 
    : DomainEvent;