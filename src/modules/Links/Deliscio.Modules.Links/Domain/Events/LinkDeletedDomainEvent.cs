using Deliscio.Modules.BuildingBlocks.Domain;
using MongoDB.Bson;

namespace Deliscio.Modules.Links.Domain.Events;
public sealed record LinkDeletedDomainEvent(
    ObjectId LinkId,
    string Title,
    string Description,
    string[] Tags,
    ObjectId UpdatedById)
    : DomainEvent;
