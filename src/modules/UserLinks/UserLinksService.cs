using Deliscio.Core.Abstracts;
using Deliscio.Modules.UserLinks.Interfaces;
using Deliscio.Modules.UserLinks.Models;
using Microsoft.Extensions.Logging;

namespace Deliscio.Modules.UserLinks;

public class UserLinksService : ServiceBase, IUserLinksService
{
    private readonly ILogger<UserLinksService> _logger;
    private readonly IUserLinksRepository _linksRepository;

    public UserLinksService(IUserLinksRepository linksRepository, ILogger<UserLinksService> logger)
    {
        _logger = logger;
        _linksRepository = linksRepository;
    }

    public Task<UserLink> GetAsync(string id, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public Task<(IEnumerable<UserLink> Results, int TotalPages, int TotalCount)> GetAsync(int pageNo = 1, int pageSize = 25, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public Task<(IEnumerable<UserLink> Results, int TotalPages, int TotalCount)> GetByDomain(string domain, int pageNo = 1, int pageSize = 25, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public Task<(IEnumerable<UserLink> Results, int TotalPages, int TotalCount)> GetByTags(IEnumerable<string> tags, int pageNo = 1, int pageSize = 25, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }
}
