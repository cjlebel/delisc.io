namespace Deliscio.Web.Admin.Models.Responses;

public record ApiResponse
{
    public bool IsSuccess { get; set; }

    public string Message { get; set; } = string.Empty;
}

public record ApiDataResponse<TData> : ApiResponse
{
    public TData? Data { get; set; }
}