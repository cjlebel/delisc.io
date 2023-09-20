using Deliscio.Core.Data.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

namespace Deliscio.Core.Data.MsSql.Interfaces;

public interface ISqlRepositoryWithTypedId<T, TId> where T : IEntityWithTypedId<TId>
{
    IDbContextTransaction BeginTransaction();


}

public interface ISqlRepository<T> : ISqlRepositoryWithTypedId<T, Guid> where T : IEntityWithTypedId<Guid>
{
}