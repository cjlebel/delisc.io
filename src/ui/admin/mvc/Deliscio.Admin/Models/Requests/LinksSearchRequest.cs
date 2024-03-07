namespace Deliscio.Admin.Models.Requests;

public record LinksSearchRequest
{
    public int PageNo { get; init; } = 1;

    public int PageSize { get; init; } = 50;

    public string SearchTerm { get; init; } = "";

    public string Domain { get; init; } = "";

    public string Tags { get; init; } = "";

    public bool IsActive { get; init; } = true;

    public bool IsFlagged { get; init; } = false;

    public bool IsDeleted { get; init; } = false;
}