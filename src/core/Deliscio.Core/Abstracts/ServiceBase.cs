using Ardalis.GuardClauses;

using Deliscio.Core.Models;

namespace Deliscio.Core.Abstracts;

public abstract class ServiceBase
{
    /// <summary>
    /// Created a Pager object, populates it with the given information, then returns it.
    /// </summary>
    /// <typeparam name="T">The type of the collection</typeparam>
    /// <param name="items">The items that will be placed into the page of results</param>
    /// <param name="pageNo">The page number of the results</param>
    /// <param name="pageSize">The max number of items per page</param>
    /// <param name="totalCount">The total number of items that this page was plucked from</param>
    /// <returns></returns>
    protected static PagedResults<T> GetPageOfResults<T>(IEnumerable<T> items, int pageNo, int pageSize, int totalCount)
    {
        var array = items.ToArray();

        Guard.Against.NullOrEmpty(array);

        var pager = new PagedResults<T>(array, pageNo, pageSize, totalCount);

        return pager;
    }
}