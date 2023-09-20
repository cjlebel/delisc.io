using Deliscio.Core.Data.MsSql;
using Deliscio.Modules.BackLinks.Data.Entities;
using Deliscio.Modules.BackLinks.Interfaces;

namespace Deliscio.Modules.BackLinks.Data.SQL;

public class BackLinkRepository : MsSqlRepository<BackLinkEntity>, IBackLinkRepository
{
    public BackLinkRepository(BackLinksDbContext context) : base(context) { }

    public Task<bool> AddOrUpdateAsync(BackLinkEntity entity, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<BackLinkEntity> Get(IEnumerable<string> ids)
    {
        if (ids == null)
            throw new ArgumentNullException(nameof(ids));

        var enumerable = (ids as string[])!.Select(id => new Guid(id)).ToList();

        if (!enumerable.Any())
            return Enumerable.Empty<BackLinkEntity>();

        return DbSet
            .Where(o => enumerable.Contains(o.Id))
            .AsEnumerable();
    }

    public IEnumerable<BackLinkEntity> Get(int page, int size, bool? isProcessed = false)
    {
        if (page <= 0)
            throw new ArgumentOutOfRangeException(nameof(page), "Value must be greater than zero.");

        if (size <= 0)
            throw new ArgumentOutOfRangeException(nameof(size), "Value must be greater than zero.");

        return DbSet
            .Where(o => isProcessed == null || o.IsProcessed == isProcessed)
            .Skip((page - 1) * size)
            .Take(size)
            .AsEnumerable();
    }

    public Task<IEnumerable<BackLinkEntity>> GetAsync(IEnumerable<string> ids)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<BackLinkEntity>> GetAsync(int page, int size, bool? isProcessed = false)
    {
        throw new NotImplementedException();
    }
}