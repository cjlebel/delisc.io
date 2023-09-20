namespace Deliscio.Apis.WebApi.Common.Responses;

public class PagedResponse<T>
{
    public IEnumerable<T> Items { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalResults { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalResults / PageSize);

    public PagedResponse(IEnumerable<T> items, int pageNumber, int pageSize, int totalResults)
    {
        Items = items;
        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalResults = totalResults;
    }
}