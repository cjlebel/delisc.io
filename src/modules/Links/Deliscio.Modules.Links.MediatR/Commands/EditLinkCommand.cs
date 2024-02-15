using Ardalis.GuardClauses;
using Deliscio.Modules.Links.Common.Interfaces;
using MediatR;

namespace Deliscio.Modules.Links.MediatR.Commands;

/// <summary>
/// Represents a command to Edit an existing link
/// </summary>
public sealed record EditLinkCommand : IRequest<(bool IsSuccess, string Message)>
{
    public Guid UpdatedById { get; }

    public Guid Id { get; }

    public string Description { get; }

    public string Title { get; }

    public string[]? Tags { get; }

    public EditLinkCommand(Guid updatedById, string id, string title, string description, string[]? tags = default)
    {
        updatedById = Guard.Against.Default(updatedById);
        Guard.Against.NullOrEmpty(id);
        Guard.Against.NullOrEmpty(title);

        UpdatedById = updatedById;
        Id = new Guid(id);
        Description = description;
        Title = title;
        Tags = tags;
    }
}

/// <summary>
/// Represents a MediatR command handler that updates an existing Link
/// </summary>
public sealed class EditLinkCommandHandler : IRequestHandler<EditLinkCommand, (bool IsSuccess, string Message)>
{
    private readonly ILinksService _service;

    public EditLinkCommandHandler(ILinksService service)
    {
        _service = service;
    }

    public async Task<(bool IsSuccess, string Message)> Handle(EditLinkCommand command, CancellationToken cancellationToken)
    {
        var rslt = await _service.UpdateLinkAsync(command.UpdatedById, command.Id, command.Title, command.Description, command.Tags, cancellationToken);

        return rslt;
    }
}