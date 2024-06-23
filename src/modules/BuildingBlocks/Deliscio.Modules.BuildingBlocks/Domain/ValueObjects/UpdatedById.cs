using Ardalis.GuardClauses;
using Habanerio.Core.BuildingBlocks.Domain;
using MongoDB.Bson;

namespace Deliscio.Modules.BuildingBlocks.Domain.ValueObjects;

public record UpdatedById : TypedIdValueBase<ObjectId>
{
    private UpdatedById(ObjectId value) : base(value) { }
    
    public static UpdatedById Create(ObjectId value)
    {
        Guard.Against.Null(value, nameof(value));

        return new UpdatedById(value);
    }

    public static UpdatedById Create(string value)
    {
        Guard.Against.NullOrEmpty(value, nameof(value));

        if (!ObjectId.TryParse(value, out var objectId))
            throw new ArgumentException("Invalid ObjectId", nameof(value));

        return new UpdatedById(objectId);
    }

    public static UpdatedById Empty() => UpdatedById.Create(ObjectId.Empty);
}