using Deliscio.Core.Models;
using Deliscio.Modules.Links.Common.Models;
using Deliscio.Modules.Links.Common.Models.Requests;

namespace Deliscio.Modules.Links.Common.Interfaces;

public interface ILinksAdminService
{
    Task<PagedResults<LinkItem>> FindAsync(FindLinksAdminRequest request, CancellationToken token = default);

    Task<Guid> AddAsync(Link link, CancellationToken token = default);

    Task<bool> DeleteAsync(Guid linkId, Guid deletedById, CancellationToken token = default);

    ValueTask<(bool IsSuccess, string Message)> UpdateLinkAsync(Guid updatedById,
        Guid id,
        string title,
        string description,
        bool isActive, string[]? tags = default,
        CancellationToken token = default);
}