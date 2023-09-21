using Deliscio.Modules.Links.Common.Models;
using MediatR;

namespace Deliscio.Modules.Links.Common.Queries;

public record GetLinkByIdQuery : IRequest<Link?>
{
    public Guid Id { get; init; }
}