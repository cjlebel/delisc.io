using Deliscio.Core.Data.Interfaces;

namespace Deliscio.Core.Data;

public class EntityBase : IEntityWithTypedId<Guid>
{
    public Guid Id { get; }

    public bool IsDeleted { get; set; }


    public Guid CreatedById { get; set; }  
    public DateTimeOffset DateCreated { get; set; }


    public Guid UpdatedById { get; set; }
    public DateTimeOffset DateUpdated { get; set; }

    DateTimeOffset? IEntityWithTypedId<Guid>.DateUpdated { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    protected EntityBase()
    {
        Id = Guid.NewGuid();

        var now = DateTimeOffset.UtcNow;

        DateCreated = now;
        DateUpdated = now;

        IsDeleted = false;
    }
}