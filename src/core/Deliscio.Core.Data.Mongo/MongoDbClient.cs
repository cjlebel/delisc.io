using Deliscio.Core.Data.Mongo.Interfaces;
using MongoDB.Driver;

namespace Deliscio.Core.Data.Mongo;

public sealed class MongoDbClient : IMongoDbClient
{
    public IMongoDatabase Database { get; }

    public MongoDbClient(MongoDbOptions options)
    {
        var client = new MongoClient(options.ConnectionString);
        Database = client.GetDatabase(options.DatabaseName);
    }

    public MongoDbClient(string connectionString, string databaseName)
    {
        var client = new MongoClient(connectionString);
        Database = client.GetDatabase(databaseName);
    }
}