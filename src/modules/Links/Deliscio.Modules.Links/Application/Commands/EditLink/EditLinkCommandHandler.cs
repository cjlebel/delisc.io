using Deliscio.Modules.BuildingBlocks.Domain.ValueObjects;
using Deliscio.Modules.Links.Domain.Links;
using Deliscio.Modules.Links.Infrastructure.Data;
using FluentResults;
using MediatR;

namespace Deliscio.Modules.Links.Application.Commands.EditLink;

public class EditLinkCommandHandler(ILinksRepository linksRepository)
    : IRequestHandler<EditLinkCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(EditLinkCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;

        if (string.IsNullOrWhiteSpace(command.UpdatedByUserId))
            return Result.Fail($"{nameof(command.UpdatedByUserId)} is required");

        if (string.IsNullOrWhiteSpace(request.LinkId))
            return Result.Fail($"{nameof(request.LinkId)} is required");

        if (string.IsNullOrWhiteSpace(request.Title))
            return Result.Fail($"nameof{request.Title} is required");

        var linkRslt = await linksRepository.GetLinkByIdAsync(request.LinkId, cancellationToken);

        if (linkRslt.IsFailed)
            return Result.Fail(linkRslt.Errors);

        var link = linkRslt.ValueOrDefault;

        if (link is null)
            return Result.Fail("Could not find the Link to update");

        link.Edit(
            new LinkTitle(request.Title), 
            new LinkDescription(request.Description), 
            request.Tags?
                .Select(t => Domain.LinkTags.LinkTag.New(t))
                .ToArray() ?? [], 
            UpdatedById.Create(command.UpdatedByUserId)
            );

        var updateRslt = await linksRepository.UpdateLinkAsync(link, command.UpdatedByUserId, cancellationToken);

        return Result.Ok(true);
    }
}