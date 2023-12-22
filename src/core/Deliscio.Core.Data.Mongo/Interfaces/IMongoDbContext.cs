using Deliscio.Core.Data.Interfaces;
using MongoDB.Driver;

namespace Deliscio.Core.Data.Mongo.Interfaces;
public interface IMongoDbContext<TDocument> : IDbContext
{
    IMongoCollection<TDocument> Collection();
}