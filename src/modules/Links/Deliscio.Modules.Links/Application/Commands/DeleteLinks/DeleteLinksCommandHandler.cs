using Deliscio.Modules.BuildingBlocks.Domain.ValueObjects;
using Deliscio.Modules.Links.Infrastructure.Data;
using FluentResults;
using MediatR;

namespace Deliscio.Modules.Links.Application.Commands.DeleteLinks;

public class DeleteLinksCommandHandler(ILinksRepository linksRepository)
    : IRequestHandler<DeleteLinksCommand, Result<string[]>>
{
    public async Task<Result<string[]>> Handle(DeleteLinksCommand command, CancellationToken cancellationToken)
    {
        if (command.LinkIds is null || command.LinkIds.Length == 0)
            return Result.Fail($"{nameof(command.LinkIds)} is required");

        if (string.IsNullOrWhiteSpace(command.DeletedByUserId))
            return Result.Fail($"{nameof(command.DeletedByUserId)} is required");

        var deletedByUserId = DeletedById.Create(command.DeletedByUserId);

        var deletedLinkIds = new List<string>();

        var dateDeleted = DateTimeOffset.UtcNow;

        foreach (var linkId in command.LinkIds)
        {
            var linkResult = await linksRepository.GetLinkByIdAsync(linkId, cancellationToken);

            if (linkResult.IsSuccess)
            {
                var link = linkResult.Value;

                // Mark the Link as deleted
                link.Delete(DeletedById.Create(command.DeletedByUserId), dateDeleted);

                var result =
                    await linksRepository.UpdateLinkAsync(link, deletedByUserId.Value.ToString(), cancellationToken);

                if (result.IsSuccess)
                {
                    deletedLinkIds.Add(linkId);
                }
            }
        }

        if (deletedLinkIds.Any())
        {
            return Result.Ok(deletedLinkIds.ToArray());
        }

        return Result.Fail("Was unable to delete any links");
    }
}