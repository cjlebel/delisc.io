using Deliscio.Core.Data.Interfaces;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Deliscio.Core.Data.Mongo;

public class MongoEntityBase : IEntityWithTypedId<ObjectId>
{
    [BsonId]
    public ObjectId Id { get; set; }

    // Allows for using DateTimeOffset as a property type (without it, it would be an Array of two values)
    [BsonRepresentation(BsonType.DateTime)]
    public DateTimeOffset DateCreated { get; set; }

    public ObjectId CreatedById { get; set; }

    [BsonRepresentation(BsonType.DateTime)]
    public DateTimeOffset DateUpdated { get; set; }

    public ObjectId UpdatedById { get; set; }

    protected MongoEntityBase()
    {
        var now = DateTimeOffset.UtcNow;

        DateCreated = now;
        DateUpdated = now;
    }
}
