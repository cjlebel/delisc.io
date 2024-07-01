using Deliscio.Modules.BuildingBlocks.Domain.ValueObjects;
using Deliscio.Modules.Links.Infrastructure.Data;
using FluentResults;
using MediatR;

namespace Deliscio.Modules.Links.Application.Commands.UnDeleteLink;

public class UnDeleteLinkCommandHandler(ILinksRepository linksRepository)
    : IRequestHandler<UnDeleteLinkCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(UnDeleteLinkCommand command, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(command.LinkId))
            return Result.Fail<bool>($"{nameof(command.LinkId)} is required");

        if (string.IsNullOrWhiteSpace(command.UpdatedByUserId))
            return Result.Fail<bool>($"{nameof(command.UpdatedByUserId)} is required");


        var linkRslt = await linksRepository.GetLinkByIdAsync(command.LinkId, cancellationToken);

        if (linkRslt.IsFailed)
            return Result.Fail(linkRslt.Errors);

        var link = linkRslt.ValueOrDefault;

        if (link is null)
            return Result.Fail("Could not find the Link to update");

        var deletedByUserId = UpdatedById.Create(command.UpdatedByUserId);

        link.UnDelete(deletedByUserId);

        var updateRslt = await linksRepository.UpdateLinkAsync(link, command.UpdatedByUserId, cancellationToken);

        return Result.Ok(true);
    }
}