using MongoDB.Bson.Serialization.Attributes;

namespace Deliscio.Core.Data.Mongo;
public abstract class BaseMongoDocument : IMongoDocument
{
    [BsonId]
    public Guid Id { get; set; }
    public bool IsDeleted { get; set; }
    public DateTimeOffset DateCreated { get; set; }
    public DateTimeOffset DateUpdated { get; set; }

    protected BaseMongoDocument()
    {
        Id = Guid.NewGuid();

        var now = DateTimeOffset.UtcNow;

        DateCreated = now;
        DateUpdated = now;
    }
}
