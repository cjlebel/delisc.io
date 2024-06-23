using Deliscio.Modules.BuildingBlocks.Domain.ValueObjects;
using Deliscio.Modules.Links.Infrastructure.Data;
using FluentResults;
using MediatR;

namespace Deliscio.Modules.Links.Application.Commands.SetLinkFlaggedState;

/// <summary>
/// Handled when the Link's flagged state is changed
/// </summary>
/// <param name="linksRepository"></param>
public class SetLinkFlaggedStateCommandHandler(ILinksRepository linksRepository)
    : IRequestHandler<SetLinkFlaggedStateCommand, Result>
{
    public async Task<Result> Handle(SetLinkFlaggedStateCommand command, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(command.LinkId))
            return Result.Fail("Link LinkId is required");

        if (string.IsNullOrWhiteSpace(command.UpdatedByUserId))
            return Result.Fail("User LinkId is required");

        var rslt = await linksRepository.GetLinkByIdAsync(command.LinkId, cancellationToken);

        if (rslt.IsFailed)
            return Result.Fail(rslt.Errors);

        if (rslt.Value is null)
            return Result.Fail("Link not found");

        var link = rslt.Value;

        link.SetFlaggedState(command.IsFlagged, UpdatedById.Create(command.UpdatedByUserId));

        rslt = await linksRepository.UpdateLinkAsync(link, command.UpdatedByUserId, cancellationToken);

        if (!rslt.IsSuccess)
            return Result.Fail(rslt.Errors);

        return Result.Ok();
    }
}