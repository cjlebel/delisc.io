using Habanerio.Core.BuildingBlocks.Domain;
using MongoDB.Bson;

namespace Deliscio.Modules.BuildingBlocks.Domain;

public record DomainEvent() : DomainEventBase<ObjectId>(ObjectId.GenerateNewId());