namespace Deliscio.Core.Models;

public record PagedResults<T>
{
    public IEnumerable<T> Items { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalResults { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalResults / PageSize);

    public PagedResults(IEnumerable<T> items, int pageNumber, int pageSize, int totalResults)
    {
        Items = items;
        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalResults = totalResults;
    }
}