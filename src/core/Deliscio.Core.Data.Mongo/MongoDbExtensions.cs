using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Bson;

namespace Deliscio.Core.Data.Mongo;

public static class MongoDbExtensions
{
    //public static void AddMongoDbSingleton(this IServiceCollection services, IConfiguration configuration)
    //{
    //    var options = configuration.GetSection("MongoDbOptions").Get<MongoDbOptions>();
    //    services.AddSingleton<MongoDbClient, MongoDbClient>(sp =>
    //    {
    //        var mongoDbOptions = options.Value;
    //        return new MongoDbClient(mongoDbOptions);
    //    });
    //}

    //public static void AddMongoDbScoped(this IServiceCollection services, IOptions<MongoDbOptions> options)
    //{
    //    services.AddScoped<MongoDbClient, MongoDbClient>(sp =>
    //    {
    //        var mongoDbOptions = options.Value;
    //        return new MongoDbClient(mongoDbOptions);
    //    });
    //}

    //public static void AddMongoDbTransient(this IServiceCollection services, IOptions<MongoDbOptions> options)
    //{
    //    services.AddTransient<MongoDbClient, MongoDbClient>(sp =>
    //    {
    //        var mongoDbOptions = options.Value;
    //        return new MongoDbClient(mongoDbOptions);
    //    });
    //}

    public static ObjectId ToObjectId(this string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentNullException(nameof(value));

        var rslt = ObjectId.TryParse(value, out var objectId);

        if (!rslt)
            throw new FormatException($"The value '{value}' is not a valid ObjectId.");

        return objectId;
    }

    public static IEnumerable<ObjectId> ToObjectIds(this IEnumerable<string> values)
    {
        var valuesArray = values?.ToArray() ?? Array.Empty<string>();

        if (!valuesArray.Any())
            throw new ArgumentNullException(nameof(values));

        foreach (var value in valuesArray)
        {
            yield return value.ToObjectId();
        }
    }
}