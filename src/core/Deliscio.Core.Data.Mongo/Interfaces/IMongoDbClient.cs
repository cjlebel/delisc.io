using MongoDB.Driver;

namespace Deliscio.Core.Data.Mongo.Interfaces;

public interface IMongoDbClient
{
    IMongoDatabase Database { get; }
}
