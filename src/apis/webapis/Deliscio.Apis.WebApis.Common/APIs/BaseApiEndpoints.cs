using Ardalis.GuardClauses;
using Deliscio.Apis.WebApi.Common.Responses;

namespace Deliscio.Apis.WebApi.Common.APIs;

public abstract class BaseApiEndpoints
{
    /// <summary>
    /// Created a Pager object, populates it with the given information, then returns it.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <param name="pageNo"></param>
    /// <param name="pageSize"></param>
    /// <param name="totalCount"></param>
    /// <returns></returns>
    public PagedResponse<T> GetPager<T>(IEnumerable<T> items, int pageNo, int pageSize, int totalCount)
    {
        Guard.Against.Null(items);

        var pager = new PagedResponse<T>(new List<T>(), pageNo, pageSize, totalCount);

        return pager;
    }
}