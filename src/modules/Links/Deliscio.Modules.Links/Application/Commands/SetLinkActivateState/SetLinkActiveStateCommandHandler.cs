using Deliscio.Modules.BuildingBlocks.Domain.ValueObjects;
using Deliscio.Modules.Links.Infrastructure.Data;
using FluentResults;
using MediatR;

namespace Deliscio.Modules.Links.Application.Commands.SetLinkActivateState;

public class SetLinkActiveStateCommandHandler(ILinksRepository linksRepository)
    : IRequestHandler<SetLinkActiveStateCommand, Result>
{
    public async Task<Result> Handle(SetLinkActiveStateCommand command, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(command.LinkId))
            return Result.Fail("Link LinkId is required");

        if (string.IsNullOrWhiteSpace(command.UpdateByUserId))
            return Result.Fail("User LinkId is required");

        var rslt = await linksRepository.GetLinkByIdAsync(command.LinkId, cancellationToken);

        if (rslt.IsFailed)
            return Result.Fail(rslt.Errors);

        if (rslt.Value is null)
            return Result.Fail("Link not found");

        var link = rslt.Value;

        link.SetActiveState(command.IsActive, UpdatedById.Create(command.UpdateByUserId));

        rslt = await linksRepository.UpdateLinkAsync(link, command.UpdateByUserId, cancellationToken);

        if (!rslt.IsSuccess)
            return Result.Fail(rslt.Errors);

        return Result.Ok();
    }
}