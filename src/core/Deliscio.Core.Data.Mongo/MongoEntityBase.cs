using Deliscio.Core.Data.Interfaces;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Deliscio.Core.Data.Mongo;

public class MongoEntityBase : IEntityWithTypedId<Guid>
{
    [BsonId]
    public Guid Id { get; set;}

    public bool IsDeleted { get; set; }

    // Allows for using DateTimeOffset as a property type (without it, it would be an Array of two values)
    [BsonRepresentation(BsonType.DateTime)]
    public DateTimeOffset DateCreated { get; set; }

    [BsonRepresentation(BsonType.DateTime)]
    public DateTimeOffset DateUpdated { get; set; }

    protected MongoEntityBase()
    {
        //Id = Guid.NewGuid();

        var now = DateTimeOffset.UtcNow;

        DateCreated = now;
        DateUpdated = now;

        IsDeleted = false;
    }
}
