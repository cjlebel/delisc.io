using Deliscio.Modules.BackLog.Models;

namespace Deliscio.Modules.BackLog.Interfaces;

public interface IBacklogService
{
    /// <summary>
    /// Attempts to add a backlog item to the data storage.
    /// If the URL already exists, the existing item will be updated.
    /// </summary>
    /// <param name="url">The URL of the backlog item.</param>
    /// <param name="title">The title of the backlog item.</param>
    /// <param name="createdByUserId">The Id of the user who is creating this item</param>
    /// <returns>If successful then true, else false</returns>
    /// Task<bool> AddBacklogItemAsync(string url, string title, string createdByUserId);
    /// <summary>
    /// Attempts to add a backlog item to the data storage.
    /// If the URL already exists, the existing item will be updated.
    /// </summary>
    /// <param name="backlogItem">The item to attempt to add</param>
    /// <returns>If successful then true, else false</returns>
    Task<bool> AddBacklogItemAsync(BacklogItem item, CancellationToken token);

    /// <summary>
    /// Attempts to add a collection of backlog items to the data storage.
    /// </summary>
    /// <param name="backlogItems"></param>
    /// <returns>The number of items that were successfully or failed to be saved</returns>
    ValueTask<(int Success, int Failed)> AddBacklogItemsAsync(IEnumerable<BacklogItem> backlogItems, CancellationToken token = default);

    /// <summary>
    /// Gets a backlog item by its id in the data storage.
    /// </summary>
    /// <param name="id">The id of the backlog item to be returned.</param>
    /// <returns>A nullable BacklogItem</returns>
    Task<BacklogItem?> GetBacklogItemAsync(string id);

    /// <summary>
    /// Gets an IEnumerable of BacklogItems from the data storage by their individual ids.
    /// </summary>
    /// <param name="ids">A collection of ids for the backlog items to be returned.</param>
    /// <returns></returns>
    Task<IEnumerable<BacklogItem>> GetBacklogItemsAsync(IEnumerable<string> ids, CancellationToken token = default);

    /// <summary>
    /// Gets an IEnumerable of BacklogItems along with the total count of items in the data storage.
    /// </summary>
    /// <param name="pageNo">The page number of results to return.</param>
    /// <param name="pageSize">Size of the page of items to be returned.</param>
    /// <param name="isProcessed">Whether or not to return all items (<c>null</c>), only items that are processed (<c>true</c>),
    /// or those that are not processed (<c>false</c>)</param>
    /// <remarks>This should return a paging object instead of a tuple</remarks>
    /// <returns>A named tuple of items and the total count of items that are available based on the isProcessed</returns>
    Task<(IEnumerable<BacklogItem> Items, int TotalCount)> GetBacklogItemsAsync(int pageNo, int pageSize = 50,
        bool? isProcessed = false);
}