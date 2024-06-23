//using Ardalis.GuardClauses;
//using Deliscio.Modules.Links.Common.Interfaces;
//using MediatR;

//namespace Deliscio.Modules.Links.MediatR.Commands;

///// <summary>
///// Represents a command to delete a link from the central repository
///// </summary>
//public class DeleteLinksCommand : IRequest<string[]>

//{
//    public string[] LinkIds { get; }

//    public string UserId { get; set; }

//    public DeleteLinksCommand(string[] linkIds, string userId)
//    {
//        Guard.Against.NullOrEmpty(linkIds);
//        Guard.Against.NullOrWhiteSpace(userId);

//        LinkIds = linkIds;
//        UserId = userId;
//    }
//}

///// <summary>
///// Represents a MediatR command handler that adds a new link to the central repository.
///// This differs from SubmitLink, as this saves the Link to the central repo, whereas Submit adds it to be verified prior to adding.
///// </summary>
//public class DeleteLinksCommandHandler : IRequestHandler<DeleteLinksCommand, string[]>
//{
//    private readonly ILinksService _service;

//    public DeleteLinksCommandHandler(ILinksService service)
//    {
//        _service = service;
//    }

//    public async Task<string[]> Handle(DeleteLinksCommand command, CancellationToken cancellationToken)
//    {
//        var deletedIds = new List<string>();

//        // This could be replaced by something in the service/repo.
//        foreach (var linkId in command.LinkIds)
//        {
//            Guard.Against.NullOrWhiteSpace(linkId);

//            if (await _service.DeleteAsync(linkId, command.UserId, cancellationToken))
//            {
//                deletedIds.Add(linkId);
//            }
//        }

//        return deletedIds.ToArray();
//    }
//}