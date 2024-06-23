using Deliscio.Modules.Links.Application.Contracts;
using FluentResults;

namespace Deliscio.Modules.Links.Application.Commands.SetLinkFlaggedState;

public class SetLinkFlaggedStateCommand(string linkId, bool isFlagged, string UpdatedByUserId) : LinksCommandBase<Result>
{
    public string LinkId { get; set; } = linkId;

    public bool IsFlagged { get; set; } = isFlagged;

    public string UpdatedByUserId { get; set; } = UpdatedByUserId;
}