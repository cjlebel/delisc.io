using Ardalis.GuardClauses;
using Deliscio.Core.Data.Mongo.Attributes;
using Deliscio.Core.Data.Mongo.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Deliscio.Core.Data.Mongo;
public class MongoDbContext<TDocument> : IMongoDbContext<TDocument>
{
    private readonly IMongoDatabase _db;

    private const string COULD_NOT_GET_OPTIONS = "Could not retrieve the options";
    //private const string COULD_NOT_GET_CONFIG = "Could not retrieve the config";
    private const string COULD_NOT_GET_CONNSTRING = "Could not retrieve the connection string";
    private const string COULD_NOT_GET_DBNAME = "Could not retrieve the database name";

    public MongoDbContext(IMongoDbClient client)
    {
        Guard.Against.Null(client, message: "Could not retrieve the client");

        _db = client.Database;
    }

    public MongoDbContext(string connectionString, string databaseName)
    {
        Guard.Against.NullOrWhiteSpace(connectionString, message: COULD_NOT_GET_CONNSTRING);
        Guard.Against.NullOrWhiteSpace(databaseName, message: COULD_NOT_GET_DBNAME);

        var client = new MongoClient(connectionString);
        _db = client.GetDatabase(databaseName);
    }

    //public MongoDbContext(IConfiguration config)
    //{
    //    Guard.Against.Null(config, message: COULD_NOT_GET_CONFIG);

    //    var connection = config["MongoDbSettings:ConnectionString"] ?? throw new ArgumentException(COULD_NOT_GET_CONNSTRING, nameof(config));
    //    var dbName = config["MongoDbSettings:DatabaseName"] ?? throw new ArgumentException(COULD_NOT_GET_DBNAME, nameof(config));

    //    var client = new MongoClient(connection);
    //    _db = client.GetDatabase(dbName);
    //}

    public MongoDbContext(IOptions<MongoDbOptions> options)
    {
        if (options?.Value == null)
            throw new ArgumentException(COULD_NOT_GET_OPTIONS, nameof(options));

        if (string.IsNullOrWhiteSpace(options.Value.ConnectionString))
            throw new ArgumentException(COULD_NOT_GET_CONNSTRING, nameof(options));

        if (string.IsNullOrWhiteSpace(options.Value.DatabaseName))
            throw new ArgumentException(COULD_NOT_GET_DBNAME, nameof(options));

        var optionValue = options.Value;


        var connection = optionValue.ConnectionString;
        var dbName = optionValue.DatabaseName;

        var client = new MongoClient(connection);
        _db = client.GetDatabase(dbName);
    }

    public IMongoCollection<TDocument> Collection()
    {
        var collection = _db.GetCollection<TDocument>(GetCollectionName(typeof(TDocument)));

        return collection;
    }

    private string GetCollectionName(Type documentType)
    {
        var name = (documentType.GetCustomAttributes(typeof(BsonCollectionAttribute), true)
            .FirstOrDefault() as BsonCollectionAttribute)?
            .CollectionName ??
            string.Empty;

        return name;
    }
}
