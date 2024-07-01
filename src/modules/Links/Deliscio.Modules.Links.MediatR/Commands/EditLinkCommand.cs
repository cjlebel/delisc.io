//using Ardalis.GuardClauses;
//using Deliscio.Modules.Links.Common.Interfaces;
//using Deliscio.Modules.Links.Common.Models.Requests;
//using MediatR;

//namespace Deliscio.Modules.Links.MediatR.Commands;

///// <summary>
///// Represents a command to Edit an existing link
///// </summary>
//public sealed record EditLinkCommand : IRequest<(bool IsSuccess, string Message)>
//{
//    public string UpdatedByUserId { get; }

//    public string Id { get; }

//    public bool IsActive { get; }

//    public string Description { get; }

//    public string Title { get; }

//    public string[]? TagsCollection { get; }

//    public EditLinkCommand(LinkEditRequest request, string updatedById)
//    {
//        Guard.Against.NullOrWhiteSpace(updatedById);
//        Guard.Against.Null(request);
//        Guard.Against.NullOrWhiteSpace(request.Id);
//        Guard.Against.NullOrWhiteSpace(request.Title);

//        UpdatedByUserId = updatedById;
//        Id = request.Id;
//        IsActive = request.IsActive;
//        Description = request.Description;
//        Title = request.Title;
//        TagsCollection = request.TagsCollection;
//    }
//}

///// <summary>
///// Represents a MediatR command handler that updates an existing Link
///// </summary>
//public sealed class EditLinkCommandHandler : IRequestHandler<EditLinkCommand, (bool IsSuccess, string Message)>
//{
//    private readonly ILinksService _service;

//    public EditLinkCommandHandler(ILinksService service)
//    {
//        _service = service;
//    }

//    public async Task<(bool IsSuccess, string Message)> Handle(EditLinkCommand command, CancellationToken cancellationToken)
//    {
//        var rslt = await _service.UpdateLinkAsync(command.UpdatedByUserId, command.Id, command.Title, command.Description, command.IsActive, command.TagsCollection, cancellationToken);

//        return rslt;
//    }
//}