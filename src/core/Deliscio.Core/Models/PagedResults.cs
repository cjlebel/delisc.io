namespace Deliscio.Core.Models;

public record PagedResults<T>
{
    public IEnumerable<T> Results { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalResults { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalResults / PageSize);

    public bool IsError { get; set; }

    public bool IsSuccess => !IsError;

    public string Message { get; set; } = string.Empty;

    public PagedResults(IEnumerable<T> results, int pageNumber, int pageSize, int totalResults)
    {
        Results = results;
        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalResults = totalResults;
    }
}