using Deliscio.Modules.Links.Common.Models;
using MediatR;

namespace Deliscio.Modules.Links.MediatR.Queries;

public sealed record GetLinkByUrlQuery : IRequest<Link?>
{
    public string Url { get; init; }

    public GetLinkByUrlQuery(Uri url)
    {
        Url = url.OriginalString;
    }
}