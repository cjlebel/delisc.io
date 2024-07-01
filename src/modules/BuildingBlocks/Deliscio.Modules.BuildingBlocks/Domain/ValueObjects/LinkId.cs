using Ardalis.GuardClauses;
using Habanerio.Core.BuildingBlocks.Domain;
using MongoDB.Bson;

namespace Deliscio.Modules.BuildingBlocks.Domain.ValueObjects;

public sealed record LinkId : TypedIdValueBase<ObjectId>
{
    private LinkId(ObjectId value) : base(value) { }

    public static LinkId Create(ObjectId value)
    {
        if (value.Equals(ObjectId.Empty))
            throw new ArgumentException("LinkId cannot be empty", nameof(value));

        return new LinkId(value);
    }

    public static LinkId Create(string value)
    {
        Guard.Against.NullOrEmpty(value, nameof(value));

        if (!ObjectId.TryParse(value, out var objectId) || objectId.Equals(ObjectId.Empty))
            throw new ArgumentException("LinkId is not a valid ObjectId", nameof(value));


        return new LinkId(objectId);
    }
}
