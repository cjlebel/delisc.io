using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

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
}