namespace Deliscio.Web.Admin.Models.Requests;

public record LinksSearchQueryRequest
{
    public string? Term { get; set; } = "";
    public string? Domain { get; set; } = "";
    public string? Tags { get; set; } = "";

    public bool? IsActive { get; set; } = false;
    public bool? IsDeleted { get; set; } = false;
    public bool? IsFlagged { get; set; } = false;

    public int? Page { get; set; } = 1;
    public int? Size { get; set; } = 50;
    public int? Offset { get; set; } = 0;
}