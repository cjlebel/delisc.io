using Ardalis.GuardClauses;
using Deliscio.Modules.Links.Application.Contracts;
using FluentResults;

namespace Deliscio.Modules.Links.Application.Commands.EditLink;

public class EditLinkCommand : LinksCommandBase<Result<bool>>
{
    public EditLinkRequest Request { get; }

    public string UpdatedByUserId { get; }

    public EditLinkCommand(EditLinkRequest request, string updatedByUserId)
    {
        Request = Guard.Against.Null(request, nameof(request));
        UpdatedByUserId = Guard.Against.NullOrWhiteSpace(updatedByUserId, nameof(updatedByUserId));
    }
}