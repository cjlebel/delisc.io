using Deliscio.Modules.Links.Common.Models;
using MediatR;

namespace Deliscio.Modules.Links.MediatR.Commands;

/// <summary>
/// Represents a command to update an existing link with the information from a queued link
/// </summary>
public sealed record UpdateLinkCommand : IRequest<Guid>
{
    public Link Link { get; set; }

    public UpdateLinkCommand(Link link)
    {
        Link = link;
    }
}