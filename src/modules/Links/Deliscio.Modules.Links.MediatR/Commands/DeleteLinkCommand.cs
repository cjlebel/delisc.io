//using Ardalis.GuardClauses;
//using Deliscio.Modules.Links.Common.Interfaces;
//using MediatR;

//namespace Deliscio.Modules.Links.MediatR.Commands;

///// <summary>
///// Represents a command to delete a link from the central repository
///// </summary>
//public class DeleteLinkCommand : IRequest<bool>

//{
//    public string LinkId { get; }

//    public string UserId { get; set; }

//    public DeleteLinkCommand(string linkId, string userId)
//    {
//        Guard.Against.NullOrWhiteSpace(linkId);
//        Guard.Against.NullOrWhiteSpace(userId);

//        LinkId = linkId;
//        UserId = userId;
//    }
//}

///// <summary>
///// Represents a MediatR command handler that adds a new link to the central repository.
///// This differs from SubmitLink, as this saves the Link to the central repo, whereas Submit adds it to be verified prior to adding.
///// </summary>
//public class DeleteLinkCommandHandler : IRequestHandler<DeleteLinkCommand, bool>
//{
//    private readonly ILinksService _service;

//    public DeleteLinkCommandHandler(ILinksService service)
//    {
//        _service = service;
//    }

//    public Task<bool> Handle(DeleteLinkCommand command, CancellationToken cancellationToken)
//    {
//        return _service.DeleteAsync(command.LinkId, command.UserId, cancellationToken);
//    }
//}