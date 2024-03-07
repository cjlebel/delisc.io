using Deliscio.Core.Models;
using Deliscio.Modules.Links.Common.Models;

namespace Deliscio.Modules.Links.Interfaces;

public interface ILinksAdminService
{
    Task<PagedResults<LinkItem>> FindAsync(string term, int pageNo = 1, int? pageSize = default,
        bool? isActive = default, bool? isFlagged = default, bool? isDeleted = false,
        CancellationToken token = default);



    //Task UpdateLinkAsync(LinkEntity linkEntity, string[]? tags = default, CancellationToken token = default);
}