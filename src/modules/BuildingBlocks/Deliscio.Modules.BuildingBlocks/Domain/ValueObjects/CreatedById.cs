using Ardalis.GuardClauses;
using Habanerio.Core.BuildingBlocks.Domain;
using MongoDB.Bson;

namespace Deliscio.Modules.BuildingBlocks.Domain.ValueObjects;

public record CreatedById : TypedIdValueBase<ObjectId>
{
    private CreatedById(ObjectId value) : base(value) { }

    public static CreatedById Create(ObjectId value)
    {
        Guard.Against.Null(value, nameof(value));

        if (value.Equals(ObjectId.Empty))
            throw new ArgumentException("CreatedById's value cannot be empty", nameof(value));

        return new CreatedById(value);
    }

    public static CreatedById Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("CreatedById's value cannot be empty", nameof(value));

        if (!ObjectId.TryParse(value, out var objectId))
            throw new ArgumentException("CreatedById's value is invalid", nameof(value));

        return new CreatedById(objectId);
    }
}