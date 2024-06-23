using Deliscio.Modules.Links.Application.Contracts;
using FluentResults;
using MediatR;

namespace Deliscio.Modules.Links.Application.Commands.SetLinkActivateState;

public class SetLinkActiveStateCommand(string linkId, bool isActive, string updateByUserId) : LinksCommandBase<Result>
{
    public string LinkId { get; set; } = linkId;

    public bool IsActive { get; set; } = isActive;

    public string UpdateByUserId { get; set; } = updateByUserId;
}