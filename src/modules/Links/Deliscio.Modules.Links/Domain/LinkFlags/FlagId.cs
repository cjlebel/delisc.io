using Habanerio.Core.BuildingBlocks.Domain;
using MongoDB.Bson;

namespace Deliscio.Modules.Links.Domain.LinkFlags;

public sealed record FlagId(ObjectId value) : TypedIdValueBase<ObjectId>(value);
