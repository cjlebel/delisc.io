using Ardalis.GuardClauses;

using Deliscio.Core.Data.Mongo;
using Deliscio.Modules.Backlog.Mappers;
using Deliscio.Modules.BackLog.Data.Entities;
using Deliscio.Modules.BackLog.Data.Mongo;
using Deliscio.Modules.BackLog.Interfaces;
using Deliscio.Modules.BackLog.Models;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using Structurizr.Annotations;

namespace Deliscio.Modules.BackLog;

[Component(Description = "The Backlog Service is responsible for saving and retrieving backlog items. This is temporary", Technology = "C#")]
public sealed class BacklogService : IBacklogService
{
    private readonly ILogger<BacklogService>? _logger;
    private readonly IBacklogRepository _repository;

    private const string COLLECTION_CANNOT_BE_NULL_OR_EMPTY = "The collection cannot be null or empty.";
    private const string ITEM_CANNOT_BE_NULL = "The item cannot be null.";
    private const string ITEM_ID_CANNOT_BE_EMPTY = "The item id cannot be null or whitespace.";
    private const string ITEMS_CANNOT_BE_MAPPED = "The items could not be mapped.";
    private const string PAGE_NUMBER_GREATER_THAN_ZERO = "The page number must be greater than zero.";
    private const string PAGE_SIZE_GREATER_THAN_ZERO = "The page size must be greater than zero.";
    private const string TITLE_CANNOT_BE_EMPTY = "The title cannot be null or whitespace.";
    private const string URL_CANNOT_BE_EMPTY = "The URL cannot be null or whitespace.";
    private const string USER_ID_CANNOT_BE_EMPTY = "The Id cannot be null or whitespace.";

    //public BacklogService(IOptions<MongoDbOptions> options, ILogger<BacklogService> logger)
    //{
    //    _logger = logger;
    //    _repository = new BacklogRepository(options, null);
    //}

    //public BacklogService(IBacklogRepository repository, ILogger<BacklogService> logger)
    //{
    //    _logger = logger;
    //    _repository = repository;
    //}


    public BacklogService(IOptions<MongoDbOptions> options, ILogger<BacklogService>? logger = default)
    {
        Guard.Against.Null(options);

        _repository = new BacklogRepository(options);
        _logger = logger;
    }

    /// <summary>
    /// Adds a BackLogItem Item to the repository asynchronously.
    /// If this already exists, then it will be updated.
    /// </summary>
    /// <param name="item">The backlog item to attempt to add or update</param>
    /// <returns>True or false depending on if it saves successfully</returns>
    public async Task<bool> AddBacklogItemAsync(BacklogItem item, CancellationToken token)
    {
        Guard.Against.Null(item, message: ITEM_CANNOT_BE_NULL);

        Guard.Against.NullOrEmpty(item.CreatedById, message: USER_ID_CANNOT_BE_EMPTY);
        Guard.Against.NullOrEmpty(item.Title, message: TITLE_CANNOT_BE_EMPTY);
        Guard.Against.NullOrEmpty(item.Url, message: URL_CANNOT_BE_EMPTY);

        return await _repository.AddOrUpdateAsync(item.Url, item.Title, item.CreatedById, token);
    }

    /// <summary>
    /// Attempts to add a collection of backlog items to the data storage.
    /// </summary>
    /// <param name="backlogItems">The collection of items to be added</param>
    /// <returns>The number of successful and failed items</returns>
    /// <exception cref="ArgumentNullException">If the collection of backlogItems is null or empty</exception>
    /// <exception cref="InvalidOperationException">If the collection of backlogItems could not be mapped</exception>
    public async ValueTask<(int Success, int Failed)> AddBacklogItemsAsync(IEnumerable<BacklogItem> backlogItems, CancellationToken token = default)
    {
        // Attempt to cast the IEnumerable to an array (need an instance before checking length).
        var items = (backlogItems as List<BacklogItem> ?? Enumerable.Empty<BacklogItem>())
            .Where(l => !string.IsNullOrWhiteSpace(l.Title) || !string.IsNullOrWhiteSpace(l.Url) || !string.IsNullOrWhiteSpace(l.CreatedById))
            .ToList();

        // Get the entities from the items. Anything that is null is discarded internally.
        List<BacklogItemEntity> entities = (Mappers.Map(items) as List<BacklogItemEntity> ?? throw new InvalidOperationException(ITEMS_CANNOT_BE_MAPPED)).ToList();

        if (!entities.Any())
            return (0, backlogItems.Count());

        var successCount = 0;

        var batchSize = 100;
        var batchCount = 0;

        await _repository.AddRangeAsync(entities, token);

        successCount = entities.Count(b => b.Id != ObjectId.Empty);

        //await Parallel.ForEachAsync(items, new ParallelOptions { MaxDegreeOfParallelism = 3 },
        //    async (item, token) =>
        //    {
        //        var entity = new BacklogItemEntity(item.Url, item.Title, item.CreatedById);

        //        batch.Add(entity);
        //        batchCount++;

        //        if (batchCount == batchSize)
        //        {
        //            await _repository.AddRangeAsync(batch, token);
        //            successCount += batch.Count(b => b.Id != Guid.Empty);

        //            batch.Clear();
        //            batchCount = 0;
        //        }
        //    });

        // Not sure which would be faster, Parallel.ForEachAsync or Task.WhenAll
        // var tasks = entities.Select(e => AddBacklogItemAsync(e.Url, e.Title, e.CreatedById.ToString())).ToList();
        // var results = await Task.WhenAll(tasks);
        // Return the number of 'true' results.
        //return results.Count(r => r);

        // Return the number of successful and calculate the number of failed items (see comment above Map)
        return (successCount, items.Count - successCount);
    }

    /// <summary>
    /// Adds a BackLogItem Item to the repository asynchronously.
    /// If this already exists, then it will be updated.
    /// </summary>
    /// <param name="url">The url for the page</param>
    /// <param name="title">The title of the page</param>
    /// <param name="createdByUserId">The id of the user who created this item</param>
    /// <exception cref="ArgumentNullException">If the url is null then the exception will be thrown</exception>

    // public async Task<bool> AddBacklogItemAsync(string url, string title, string createdByUserId)
    // {
    //     if (string.IsNullOrWhiteSpace(createdByUserId))
    //         throw new ArgumentNullException(nameof(createdByUserId), USER_ID_CANNOT_BE_EMPTY);
    //
    //     if (string.IsNullOrWhiteSpace(title))
    //         throw new ArgumentNullException(nameof(title), TITLE_CANNOT_BE_EMPTY);
    //
    //     if (string.IsNullOrWhiteSpace(url))
    //         throw new ArgumentNullException(nameof(url), URL_CANNOT_BE_EMPTY);
    //
    //     return await _repository.AddOrUpdateAsync(url, title, createdByUserId);
    // }



    /// <summary>
    /// Gets an individual nullable back link asynchronous.
    /// </summary>
    /// <param name="id">The id of the back link to retrieve.</param>
    /// <returns></returns>
    /// <exception cref="System.ArgumentNullException">id - Value cannot be null or whitespace.</exception>
    public async Task<BacklogItem?> GetBacklogItemAsync(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
            throw new ArgumentNullException(nameof(id), ITEM_ID_CANNOT_BE_EMPTY);

        var entity = await _repository.GetAsync(ObjectId.Parse(id));

        return entity != null ? Mappers.Map(entity) : null;
    }


    /// <summary>
    /// Gets a collection of back links by their ids asynchronously.
    /// </summary>
    /// <param name="ids">The ids.</param>
    /// <returns>
    /// If ids are not null then an IEnumerable of Backlog items will be returned.<br />
    /// If ids are empty then an empty IEnumerable of Backlog items will be returned instead
    /// </returns>
    /// <exception cref="System.ArgumentNullException">If ids are null</exception>
    public async Task<IEnumerable<BacklogItem>> GetBacklogItemsAsync(IEnumerable<string> ids, CancellationToken token = default)
    {
        if (ids == null)
            throw new ArgumentNullException(nameof(ids));

        var guidIds = ids.Select(ObjectId.Parse).ToList();

        if (!guidIds.Any())
            return Enumerable.Empty<BacklogItem>();

        var entities = await _repository.GetAsync(guidIds, token);

        return Mappers.Map(entities);
    }

    /// <summary>
    /// Gets a page collection of backlog items asynchronously.
    /// </summary>
    /// <param name="pageNo">The page of items to be returned</param>
    /// <param name="pageSize">The size of the page</param>
    /// <param name="isProcessed">Filter by whether items are process or not, or all items (null)</param>
    /// <returns>A page of results along with the total number of all items based on the isProcessed flag</returns>
    /// <exception cref="ArgumentOutOfRangeException">If the pageNo is less than or equal to 0</exception>
    /// <exception cref="ArgumentOutOfRangeException">If the pageSize is less than or equal to 0</exception>
    public async Task<(IEnumerable<BacklogItem> Items, int TotalCount)> GetBacklogItemsAsync(int pageNo,
        int pageSize = 50, bool? isProcessed = false)
    {
        if (pageNo <= 0)
            throw new ArgumentOutOfRangeException(nameof(pageNo), PAGE_NUMBER_GREATER_THAN_ZERO);

        if (pageSize <= 0)
            throw new ArgumentOutOfRangeException(nameof(pageSize), PAGE_SIZE_GREATER_THAN_ZERO);

        var results = await _repository.GetAsync(pageNo, pageSize, isProcessed);

        return (Mappers.Map(results.Items), results.TotalCount);
    }

    public async Task RemoveAll(CancellationToken token)
    {
        await _repository.RemoveAllAsync(token);
    }
}