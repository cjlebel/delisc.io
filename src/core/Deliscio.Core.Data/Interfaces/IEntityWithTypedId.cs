namespace Deliscio.Core.Data.Interfaces;

public interface IEntityWithTypedId<TId>
{
    TId Id { get; }


    TId CreatedById { get; set; }

    DateTimeOffset DateCreated { get; set; }


    TId UpdatedById { get; set; }

    DateTimeOffset? DateUpdated { get; set; }
}