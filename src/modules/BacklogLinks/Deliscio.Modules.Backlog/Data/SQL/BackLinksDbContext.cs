using Deliscio.Core.Data.MsSql;

using Microsoft.EntityFrameworkCore;

namespace Deliscio.Modules.BackLinks.Data.SQL;

public class BackLinksDbContext : MsSqlDbContext
{
    public BackLinksDbContext(DbContextOptions options) : base(options)
    {
    }
}