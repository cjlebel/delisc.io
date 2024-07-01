using Deliscio.Modules.Links.Application.Dtos;
using MediatR;

namespace Deliscio.Modules.Links.MediatR.Queries;

public sealed record GetLinkByUrlQuery : IRequest<LinkDto?>
{
    public string Url { get; init; }

    public GetLinkByUrlQuery(Uri url)
    {
        Url = url.OriginalString;
    }
}