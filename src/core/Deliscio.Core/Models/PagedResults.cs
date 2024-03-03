namespace Deliscio.Core.Models;

public record PagedResults<T>
{
    public IEnumerable<T> Results { get; set; } = new List<T>();
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalResults { get; set; }
    public int TotalPages => PageSize > 0 ? (int)Math.Ceiling((double)TotalResults / PageSize) : 0;

    public bool IsError { get; set; }

    public bool IsSuccess => !IsError;

    public string Message { get; set; } = string.Empty;

    // Needed for deserialization
    public PagedResults() { }

    public PagedResults(IEnumerable<T> results, int pageNumber, int pageSize, int totalResults)
    {
        Results = results.ToList();
        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalResults = totalResults;
    }
}