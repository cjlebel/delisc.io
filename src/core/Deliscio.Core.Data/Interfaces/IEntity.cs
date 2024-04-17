namespace Deliscio.Core.Data.Interfaces;
public interface IEntityWithTypedId<out TId>
{
    TId Id { get; }

    //bool IsDeleted { get; set; }

    DateTimeOffset DateCreated { get; set; }

    DateTimeOffset? DateUpdated { get; set; }
}