using Microsoft.EntityFrameworkCore;

namespace Deliscio.Core.Data.MsSql.Interfaces;

public interface IModelBuilder
{
    void Build(ModelBuilder modelBuilder);
}