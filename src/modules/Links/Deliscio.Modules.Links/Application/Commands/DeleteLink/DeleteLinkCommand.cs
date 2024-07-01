using Deliscio.Modules.Links.Application.Contracts;
using FluentResults;
using MediatR;

namespace Deliscio.Modules.Links.Application.Commands.DeleteLink;

/// <summary>
/// Marks a Link as deleted
/// </summary>
public class DeleteLinkCommand : LinksCommandBase<Result<bool>>
{
    public string LinkId { get; set; }

    public string DeletedByUserId { get; set; }

    public DeleteLinkCommand(string linkId, string deletedByUserId)
    {
        LinkId = linkId;
        DeletedByUserId = deletedByUserId;
    }
}