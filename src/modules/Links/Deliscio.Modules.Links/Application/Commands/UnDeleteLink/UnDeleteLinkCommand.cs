using Deliscio.Modules.Links.Application.Contracts;
using FluentResults;
using MediatR;

namespace Deliscio.Modules.Links.Application.Commands.UnDeleteLink;

/// <summary>
/// Marks a Link as deleted
/// </summary>
public class UnDeleteLinkCommand : LinksCommandBase<Result<bool>>
{
    public string LinkId { get; set; }

    public string UpdatedByUserId { get; set; }

    public UnDeleteLinkCommand(string linkId, string updatedByUserId)
    {
        LinkId = linkId;
        UpdatedByUserId = updatedByUserId;
    }
}