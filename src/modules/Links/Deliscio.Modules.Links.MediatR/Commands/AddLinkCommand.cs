using Deliscio.Modules.Links.Common.Models;
using MediatR;

namespace Deliscio.Modules.Links.MediatR.Commands;

/// <summary>
/// Represents a command to save the link to the central repository
/// </summary>
public sealed record AddLinkCommand : IRequest<Guid>
{
    public Link Link { get; }

    public AddLinkCommand(Link link)
    {
        Link = link;
    }
}