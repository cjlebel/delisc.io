using Deliscio.Core.Data.Interfaces;

namespace Deliscio.Core.Data;

public class EntityBase : IEntityWithTypedId<Guid>
{
    public Guid Id { get; }

    public bool IsDeleted { get; set; }

    public DateTimeOffset DateCreated { get; set; }

    public DateTimeOffset? DateUpdated { get; set; }

    protected EntityBase()
    {
        Id = Guid.NewGuid();

        var now = DateTimeOffset.UtcNow;

        DateCreated = now;
        DateUpdated = now;

        IsDeleted = false;
    }
}