using System.Collections.Immutable;

namespace Deliscio.Core.Models;

public class PagedResults<TValue>
{
    public int PageNumber { get; set; }

    public int PageSize { get; set; }

    public int Offset { get; set; }

    public int TotalResults { get; set; }

    public int TotalPages => PageSize > 0 ? (int)Math.Ceiling((double)TotalResults / PageSize) : 0;

    public IReadOnlyList<TValue> Items { get; set; } = [];

    public PagedResults() { }

    public PagedResults(IEnumerable<TValue> items, int pageNo, int pageSize, int totalResults, int offset = 0)
    {
        Items = items.ToImmutableList();
        
        PageNumber = pageNo;
        PageSize = pageSize;
        Offset = offset;

        TotalResults = totalResults;
    }

    //public static PagedResults<TValue> Empty(int pageNo, int pageSize)
    //{
    //    return new PagedResults<TValue> { Items = [], PageNumber = pageNo, PageSize = pageSize, TotalResults = 0 };
    //}

    //public new static PagedResults<TValue> Fail(string message)
    //{
    //    var rslt = new PagedResults<TValue>();

    //    rslt.WithError(message);

    //    return rslt;
    //}

    //public static PagedResults<TValue> Fail(Exception ex)
    //{
    //    var rslt = new PagedResults<TValue>();

    //    rslt.WithError(ex.Message);

    //    return rslt;
    //}

    ///// <summary>
    ///// Use this when you have already paged the items and know the total results count
    ///// </summary>
    ///// <typeparam name="T"></typeparam>
    ///// <param name="items"></param>
    ///// <param name="pageNo"></param>
    ///// <param name="pageSize"></param>
    ///// <param name="totalResults"></param>
    ///// <returns></returns>
    //public static PagedResults<TValue> Ok(IEnumerable<TValue> items, int pageNo, int pageSize, int totalResults)
    //{
    //    var result = new PagedResults<TValue> { Items = items, PageNumber = pageNo, PageSize = pageSize, TotalResults = totalResults };

    //    return result;
    //}

    ///// <summary>
    ///// Use this when you have an enumerable of ALL possible items and you want to page them.
    ///// </summary>
    ///// <typeparam name="T"></typeparam>
    ///// <param name="items"></param>
    ///// <param name="pageNo"></param>
    ///// <param name="pageSize"></param>
    ///// <returns></returns>
    //public static PagedResults<TValue> Ok(IEnumerable<TValue> items, int pageNo, int pageSize)
    //{
    //    if (items.TryGetNonEnumeratedCount(out var totalResults))
    //    {
    //        var pageOfItems = items.Skip((pageNo - 1) * pageSize).Take(pageSize).AsEnumerable();

    //        var result = new PagedResults<TValue> { Items = pageOfItems, PageNumber = pageNo, PageSize = pageSize, TotalResults = totalResults };

    //        return result;
    //    }

    //    return new PagedResults<TValue> { Items = [], PageNumber = pageNo, PageSize = pageSize, TotalResults = 0 };

    //}

    //public static PagedResults<TValue> Ok(IEnumerable<TValue> items)
    //{
    //    if (items.TryGetNonEnumeratedCount(out var totalResults))
    //    {
    //        var result = new PagedResults<TValue> { Items = items, PageNumber = 1, PageSize = totalResults, TotalResults = totalResults };

    //        return result;
    //    }

    //    return new PagedResults<TValue> { Items = [], PageNumber = 1, PageSize = 0, TotalResults = 0 };

    //}

    //public static PagedResults<TValue> Ok(PagedResults<TValue> results)
    //{
    //    return new PagedResults<TValue> { Items = results.Items, PageNumber = results.PageNumber, PageSize = results.PageSize, TotalResults = results.TotalResults };
    //}


}


//public record PagedResults<T> : Results<IEnumerable<T>> where T : class?
//{
//    public IEnumerable<T> Items { get { return base.Item ?? Enumerable.Empty<T>(); } }

//    public int PageNumber { get; set; }

//    public int PageSize { get; set; }

//    public int TotalResults { get; set; }

//    public int TotalPages => PageSize > 0 ? (int)Math.Ceiling((double)TotalResults / PageSize) : 0;

//    // Needed for deserialization
//    private PagedResults() { }

//    private PagedResults(IEnumerable<T>? results, int pageNumber, int pageSize, int totalResults)
//    {
//        PageNumber = pageNumber;
//        PageSize = pageSize;
//        TotalResults = totalResults;

//        base.Item = results ?? [];
//    }

//    public static PagedResults<T> Ok(IEnumerable<T> items, int pageNumber, int pageSize, int totalResults)
//    {
//        return new PagedResults<T>(items, pageNumber, pageSize, totalResults) { IsError = false };
//    }

//    public static PagedResults

//    public static new PagedResults<T> Fail(string message)
//    {
//        return new PagedResults<T> { IsError = true, Message = message };
//    }

//    public static new PagedResults<T> Fail(Exception ex)
//    {
//        return new PagedResults<T> { IsError = true, Message = ex.Message };
//    }
//}