using Deliscio.Modules.BuildingBlocks.Domain.ValueObjects;
using Deliscio.Modules.Links.Infrastructure.Data;
using FluentResults;
using MediatR;

namespace Deliscio.Modules.Links.Application.Commands.DeleteLink;

public class DeleteLinkCommandHandler(ILinksRepository linksRepository)
    : IRequestHandler<DeleteLinkCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(DeleteLinkCommand command, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(command.LinkId))
            return Result.Fail<bool>($"{nameof(command.LinkId)} is required");

        if (string.IsNullOrWhiteSpace(command.DeletedByUserId))
            return Result.Fail<bool>($"{nameof(command.DeletedByUserId)} is required");


        var linkRslt = await linksRepository.GetLinkByIdAsync(command.LinkId, cancellationToken);

        if (linkRslt.IsFailed)
            return Result.Fail(linkRslt.Errors);

        var link = linkRslt.ValueOrDefault;

        if (link is null)
            return Result.Fail("Could not find the Link to update");

        var deletedByUserId = DeletedById.Create(command.DeletedByUserId);

        link.Delete(deletedByUserId);

        var updateRslt = await linksRepository.UpdateLinkAsync(link, command.DeletedByUserId, cancellationToken);

        return Result.Ok(true);
    }
}