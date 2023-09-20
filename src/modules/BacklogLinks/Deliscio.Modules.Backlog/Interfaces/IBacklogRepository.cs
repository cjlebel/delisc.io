using Deliscio.Core.Data.Interfaces;
using Deliscio.Modules.BackLog.Data.Entities;

namespace Deliscio.Modules.BackLog.Interfaces;

public interface IBacklogRepository : IRepository<BacklogItemEntity>
{
    ///// <summary>
    ///// Gets a collection of one or more backlinks by their id.
    ///// </summary>
    ///// <param name="ids">The ids.</param>
    ///// <returns></returns>
    //IEnumerable<BackLinkEntity> Get(IEnumerable<string> ids);

    ///// <summary>
    ///// Gets a collection of one or more backlinks by their id.
    ///// </summary>
    ///// <param name="ids">The ids.</param>
    ///// <returns></returns>
    //Task<IEnumerable<BackLinkEntity>> GetAsync(IEnumerable<string> ids);

    /// <summary>
    /// Adds an individual backlink to the repository.
    /// </summary>
    /// <param name="url">The url of the link.</param>
    /// <param name="title">The title of the link</param>
    /// <param name="createById">The id of the user who is creating the link</param>
    /// <param name="token">The token.</param>
    /// <returns>The Id of the new backlink is returned</returns>
    ValueTask<Guid> AddAsync(string url, string title, string createById, CancellationToken token = default);

    //ValueTask<IEnumerable<Guid>> AddRangeAsync(IEnumerable<BacklogItemEntity> entities, CancellationToken token);

    /// <summary>
    /// If the backlink's url is already in the repository, update it. Otherwise, add it.
    /// </summary>
    /// <param name="url">The url of the link.</param>
    /// <param name="title">The title of the link</param>
    /// <param name="createById">The id of the user who is creating the link</param>
    /// <param name="token">The token.</param>
    /// <returns>True if successful, false if not</returns>
    ValueTask<bool> AddOrUpdateAsync(string url, string title, string createById, CancellationToken token = default);

    /// <summary>
    /// Checks to see if the url already exists in the repository.
    /// </summary>
    /// <param name="url"></param>
    /// <param name="token"></param>
    /// <returns>If the url can be found, then its Id is returned, else, Guid.Empty</returns>
    ValueTask<Guid> Exists(string url, CancellationToken token);

    ///// <summary>
    ///// Gets a collection of backlinks by page and size.
    ///// </summary>
    ///// <param name="page">The page number.</param>
    ///// <param name="size">The size of the page.</param>
    ///// <param name="isProcessed">(Optional)Whether or not to return only processed, unprocessed, or either</param>
    ///// <returns></returns>
    // IEnumerable<BacklogItemEntity> Get(int page, int size, bool? isProcessed = false);

    /// <summary>
    /// Gets a collection of backlinks by page and size.
    /// </summary>
    /// <param name="page">The page number.</param>
    /// <param name="size">The size of the page.</param>
    /// <param name="isProcessed">(Optional)Whether or not to return only processed, unprocessed, or either</param>
    /// <returns></returns>
    ValueTask<(IEnumerable<BacklogItemEntity> Items, int TotalCount)> GetAsync(int page, int size, bool? isProcessed = false);

    Task RemoveAllAsync(CancellationToken token = default);
}