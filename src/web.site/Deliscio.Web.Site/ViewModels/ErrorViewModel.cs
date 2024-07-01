namespace Deliscio.Web.Site.ViewModels;

public record ErrorViewModel : BasePageViewModel
{
    public string? RequestId { get; set; }

    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
}
