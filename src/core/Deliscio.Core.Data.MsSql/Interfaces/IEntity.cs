namespace Deliscio.Core.Data.MsSql.Interfaces;

public interface IEntityWithTypedId<TId>
{
    TId Id { get; }
}