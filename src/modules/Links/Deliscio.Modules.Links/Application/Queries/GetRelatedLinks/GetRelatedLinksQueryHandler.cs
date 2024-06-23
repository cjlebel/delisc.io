using Ardalis.GuardClauses;
using Deliscio.Modules.Links.Application.Dtos;
using Deliscio.Modules.Links.Infrastructure.Data;
using FluentResults;
using MediatR;

namespace Deliscio.Modules.Links.Application.Queries.GetRelatedLinks;

public class GetRelatedLinksQueryHandler(ILinksRepository linksRepository) : IRequestHandler<GetRelatedLinksQuery, Result<RelatedLinkDto[]>>
{
    public async Task<Result<RelatedLinkDto[]>> Handle(GetRelatedLinksQuery query, CancellationToken cancellationToken)
    {
        Guard.Against.NullOrEmpty(query.LinkId);

        var newCount = query.Count ?? 25;

        Guard.Against.NegativeOrZero(newCount);

        var linkResult = await linksRepository.GetLinkByIdAsync(query.LinkId, cancellationToken);

        if (linkResult.IsFailed)
            return Result.Fail(linkResult.Errors);

        var link = linkResult.Value;

        var domain = link.Domain.Value;
        var linkTags = link.TagsCollection.Tags?.Select(t=>t.Name).ToArray() ?? [];

        // If IsAdmin, then return all links
        // Else, only show publicly visible links
        bool? isActive = query.IsAdmin ? null : true;
        bool? isFlagged = query.IsAdmin ? null : false;
        bool? isDeleted = query.IsAdmin ? null : false;

        var relatedLinks = new List<RelatedLinkDto>();
        var tasks = new List<Task<Result<(IReadOnlyList<Domain.Links.Link> Results, int TotalPages, int TotalCount)>>>();

        // Task of Links by Domain
        if (!string.IsNullOrWhiteSpace(domain))
        {
          var relatedLinksByDomain = linksRepository.FindLinksAsync(string.Empty, [], domain,
                1, 50, 0,
                isActive: isActive,
                isFlagged: isFlagged,
                isDeleted: isDeleted,
                cancellationToken);

          tasks.Add(relatedLinksByDomain);
        }

        // Task of Links by TagCollection
        if (linkTags.Any())
        {
            var relatedLinksByTags = linksRepository.FindLinksAsync(string.Empty, 
                linkTags, string.Empty,
                1, 50, 0,
                isActive: isActive,
                isFlagged: isFlagged,
                isDeleted: isDeleted,
                cancellationToken);

            tasks.Add(relatedLinksByTags);
        }


        var taskResults = await Task.WhenAll(tasks);

        foreach(var taskResult in taskResults)
        {
            if (taskResult.IsSuccess)
            {
                var taskResultLinks = taskResult.Value.Results
                    .Select(l=> 
                        new RelatedLinkDto(
                            l.Id.Value.ToString(), 
                            l.Title.Value, 
                            l.Domain.Value, 
                            l.IsActive, 
                            l.IsDeleted, 
                            l.IsFlagged, 
                            l.DateCreated, 
                            l.DateUpdated)
                    ).ToList();

                relatedLinks.AddRange(taskResultLinks);
            }
        }


        //// First try to get links that have the same tags
        //if (linkTags.Length > 0)
        //{
        //    var results = await LinksRepository.GetLinksByTagsAsync(linkTags, 1, newCount, token);

        //    if (results.Results.TryGetNonEnumeratedCount(out int resultsCount) && resultsCount > 0)
        //        linkItems = Mapper.Map<LinkItem>(results.Results).ToArray();
        //}

        //// Then try to get links that have the same domain
        //if (linkItems.Length < newCount)
        //{
        //    var results = await linksRepository..GetLinksByDomainAsync(link.Domain, 1, newCount, token);

        //    if (results.Results.TryGetNonEnumeratedCount(out int resultsCount) && resultsCount > 0)
        //        linkItems = Mapper.Map<LinkItem>(results.Results).ToArray();
        //}

        return relatedLinks.Take(newCount).ToArray();
    }
}