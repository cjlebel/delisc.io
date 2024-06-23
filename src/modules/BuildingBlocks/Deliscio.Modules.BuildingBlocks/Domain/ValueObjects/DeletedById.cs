using Ardalis.GuardClauses;
using Habanerio.Core.BuildingBlocks.Domain;
using MongoDB.Bson;

namespace Deliscio.Modules.BuildingBlocks.Domain.ValueObjects;

public record DeletedById : TypedIdValueBase<ObjectId>
{
    private DeletedById(ObjectId value) : base(value) { }

    public static DeletedById Create(ObjectId value)
    {
        Guard.Against.Null(value, nameof(value));

        return new DeletedById(value);
    }

    public static DeletedById Create(string value)
    {
        Guard.Against.NullOrEmpty(value, nameof(value));

        if (!ObjectId.TryParse(value, out var objectId))
            throw new ArgumentException("Invalid ObjectId", nameof(value));

        return new DeletedById(objectId);
    }

    public static DeletedById Empty() => DeletedById.Create(ObjectId.Empty);
}