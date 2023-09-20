using Deliscio.Core.Data.Interfaces;

namespace Deliscio.Core.Data.Mongo;
public interface IMongoDocument : IEntityWithTypedId<Guid>
{
    Guid Id { get; set; }

    bool IsDeleted { get; set; }

    DateTimeOffset DateCreated { get; set; }

    DateTimeOffset DateUpdated { get; set; }
}