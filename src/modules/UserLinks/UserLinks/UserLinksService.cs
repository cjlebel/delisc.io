using System.Linq.Expressions;
using Ardalis.GuardClauses;
using Deliscio.Core.Abstracts;
using Deliscio.Core.Data.Mongo;
using Deliscio.Core.Models;
using Deliscio.Modules.UserLinks.Common.Interfaces;
using Deliscio.Modules.UserLinks.Common.Models;
using Deliscio.Modules.UserLinks.Data.Entities;
using Deliscio.Modules.UserLinks.Data.Mongo;
using Deliscio.Modules.UserLinks.Interfaces;
using Deliscio.Modules.UserLinks.Mappers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Structurizr.Annotations;

namespace Deliscio.Modules.UserLinks;

[Component(Description = "Deliscio service that deals with the user's saved Links", Technology = "C#")]
[UsedBySoftwareSystem("Deliscio.Apis.WebApi", Description = "User Service")]
public class UserLinksService : ServiceBase, IUserLinksService
{
    private readonly ILogger<UserLinksService> _logger;
    private readonly IUserLinksRepository _repository;

    //public UserLinksService(IUserLinksRepository repository, ILogger<UserLinksService> logger)
    //{
    //    Guard.Against.Null(repository);
    //    Guard.Against.Null(logger);

    //    _logger = logger;
    //    _repository = repository;
    //}

    //public UserLinksService(MongoDbClient client, ILogger<UserLinksService> logger)
    //{
    //    Guard.Against.Null(client);
    //    Guard.Against.Null(logger);

    //    _repository = new UserLinksRepository(client);
    //    _logger = logger;
    //}

    public UserLinksService(IOptions<MongoDbOptions> options, ILogger<UserLinksService> logger)
    {
        Guard.Against.Null(options);
        Guard.Against.Null(logger);

        _repository = new UserLinksRepository(options);
        _logger = logger;
    }

    /// <summary>
    /// Adds an existing link in the central repository to the user's collection of links
    /// </summary>
    /// <param name="userId">The id of the user to add the link to</param>
    /// <param name="linkId">The id of the link to associate with the user</param>
    /// <param name="title">The title that the user wants to use for this link (this does not affect the underlying link)</param>
    /// <param name="tags">A collection of tags that the user wants to include with their copy of this link</param>
    /// <param name="isPrivate">Whether or not this link will be visible to all (this does not affect the underlying link)</param>
    /// <returns></returns>
    /// <exception cref="ArgumentException">
    /// Thrown if the <paramref name="userId"/> or <paramref name="linkId"/> are not valid Guids
    /// </exception>
    public async Task<Guid> AddAsync(Guid userId, Guid linkId, string title = "", string[]? tags = default, bool isPrivate = false, CancellationToken token = default)
    {
        Guard.Against.NullOrEmpty(userId);
        Guard.Against.NullOrEmpty(linkId);

        var arrTags = tags?.Select(UserLinkTag.Create) ?? Array.Empty<UserLinkTag>();

        var userLink = UserLinkEntity.Create(userId, linkId, title, Mapper.Map(arrTags).ToArray(), isPrivate);

        await _repository.AddAsync(userLink, token);

        return userLink.Id;
    }

    //public async Task<Guid> AddAsync(Guid userId, UserLink link, CancellationToken token = default)
    //{
    //    Guard.Against.NullOrEmpty(userId);
    //    Guard.Against.Null(link);

    //    var entity = Mapper.Map(link);

    //    if (entity == null)
    //    {
    //        _logger.LogError("Unable to map UserLink to UserLinkEntity");
    //        return Guid.Empty;
    //    }

    //    await _repository.AddAsync(entity, token);

    //    return entity.Id;
    //}

    /// <summary>
    /// Gets an individual link that belongs to the user
    /// </summary>
    /// <param name="userId">The id of the user for which the link belongs to</param>
    /// <param name="linkId">The id of the link to retrieve</param>
    /// <param name="token"></param>
    /// <returns></returns>
    public async Task<UserLink?> GetAsync(Guid userId, Guid linkId, CancellationToken token = default)
    {
        Guard.Against.NullOrEmpty(userId);
        Guard.Against.NullOrEmpty(linkId);

        var result = await _repository.GetAsync(userId, linkId, token);

        var link = Mapper.Map(result);

        return link;
    }

    /// <summary>
    /// Gets a page of links for which belong to the user
    /// </summary>
    /// <param name="userId">The id of the user for which the links belong to</param>
    /// <param name="pageNo">The page of results to retrieve</param>
    /// <param name="pageSize">The size of the page of results</param>
    /// <param name="token"></param>
    /// <returns></returns>
    public async Task<PagedResults<UserLink>> GetAsync(Guid userId, int pageNo = 1, int pageSize = 25, CancellationToken token = default)
    {
        // TODO: Refactor so that we can use the Find method throughout the application.
        var rslts = await Find(l => l.UserId == userId, pageNo, pageSize, token);

        return GetPageOfResults(rslts.Results, pageNo, pageSize, rslts.TotalCount);
    }

    public Task<PagedResults<UserLink>> GetByDomainAsync(Guid userId, string domain, int pageNo = 1, int pageSize = 25, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public Task<PagedResults<UserLink>> GetByTagsAsync(Guid userId, IEnumerable<string> tags, int pageNo = 1, int pageSize = 25, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public async Task<UserLinkTag> GetTagAsync(Guid userId, int count = 25, string[]? tags = default, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Helper method to retrieve links from the repository
    /// </summary>
    /// <param name="predicate">The filter to apply to the links</param>
    /// <param name="pageNo">The current number of the page of results</param>
    /// <param name="pageSize">The size of the page - max number of results to return</param>
    /// <param name="token">The cancellation token</param>
    /// <returns>An IEnumerable of Link models, along with the total number of pages of results, and the total number of all results for this filter</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if either pageNo or pageSize are less than 1</exception>
    private async Task<(IEnumerable<UserLink> Results, int TotalPages, int TotalCount)> Find(Expression<Func<UserLinkEntity, bool>> predicate, int pageNo = 1, int pageSize = 25, CancellationToken token = default)
    {
        Guard.Against.NegativeOrZero(pageNo);
        Guard.Against.NegativeOrZero(pageSize);

        var results = await _repository.FindAsync(predicate, pageNo, pageSize, token);

        if (!results.Results.Any())
            return (Enumerable.Empty<UserLink>(), 0, 0);

        var links = Mapper.Map(results.Results);

        return (links, results.TotalPages, results.TotalCount);
    }
}
