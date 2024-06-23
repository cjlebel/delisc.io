//using Ardalis.GuardClauses;
//using Deliscio.Modules.Links.Common.Interfaces;
//using Deliscio.Modules.Links.Common.Models;
//using MediatR;

//namespace Deliscio.Modules.Links.MediatR.Queries;

//public sealed record GetLinkByIdQuery : IRequest<Link?>
//{
//    public string Id { get; init; }

//    public GetLinkByIdQuery(string id)
//    {
//        Guard.Against.NullOrEmpty(id);

//        Id = id;
//    }
//}

///// <summary>
///// Handles getting a single link from the central repository, by the link's id
///// </summary>
//public class GetLinkByIdQueryHandler : IRequestHandler<GetLinkByIdQuery, Link?>
//{
//    private readonly ILinksService _linksService;

//    public GetLinkByIdQueryHandler(ILinksService linksService)
//    {
//        _linksService = linksService;
//    }

//    public async Task<Link?> Handle(GetLinkByIdQuery command, CancellationToken cancellationToken)
//    {
//        var link = await _linksService.GetLinkByIdAsync(command.Id, cancellationToken);

//        return link;
//    }
//}