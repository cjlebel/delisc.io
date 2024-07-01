using Deliscio.Modules.Links.Application.Contracts;
using FluentResults;

namespace Deliscio.Modules.Links.Application.Commands.DeleteLinks;

/// <summary>
/// Marks a Link as deleted
/// </summary>
public class DeleteLinksCommand(string[] linkIds, string deletedByUserId) : LinksCommandBase<Result<string[]>>
{
    public string[] LinkIds { get; set; } = linkIds;

    public string DeletedByUserId { get; set; } = deletedByUserId;
}