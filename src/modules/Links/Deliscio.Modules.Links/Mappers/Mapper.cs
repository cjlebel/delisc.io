using Deliscio.Modules.Links.Common.Models;
using Deliscio.Modules.Links.Data.Entities;

namespace Deliscio.Modules.Links.Mappers;

internal static class Mapper
{
    internal static LinkEntity? Map(Link? link)
    {
        if (link is null)
            return null;

        var entity = new LinkEntity(new Guid(link.Id), link.Url, link.Title)
        {
            Description = link.Description,
            Domain = link.Domain,
            ImageUrl = link.ImageUrl,
            Keywords = link.Keywords ?? Array.Empty<string>(),
            //IsExcluded = link.IsExcluded,
            //IsFlagged = link.IsFlagged,
            Tags = Map(link.Tags).ToList(),
            Title = link.Title,

            SubmittedById = new Guid(link.SubmittedById),
            DateCreated = link.DateCreated,
            DateUpdated = link.DateUpdated
        };

        return entity;
    }


    internal static Link? Map(LinkEntity? entity)
    {
        if (entity == null)
            return null;

        var model = new Link(entity.Id, entity.Url, entity.SubmittedById)
        {
            Description = entity.Description,
            Domain = entity.Domain,
            ImageUrl = entity.ImageUrl,
            //IsExcluded = entity.IsExcluded,
            //IsFlagged = entity.IsFlagged,
            Tags = Map(entity.Tags).ToList(),
            Title = entity.Title,

            DateCreated = entity.DateCreated,
            DateUpdated = entity.DateUpdated,
        };

        return model;
    }

    /// <summary>
    /// Gets an IEnumerable of Link from the collection of LinkEntities.
    /// If any of the converted LinkEntities ends up being null, then they are discarded from the final results.
    /// </summary>
    /// <param name="entities"></param>
    /// <returns>IEnumerable of non-nullable Link</returns>
    internal static IEnumerable<Link> Map(IEnumerable<LinkEntity>? entities)
    {
        if (entities == null)
            return Enumerable.Empty<Link>();

        var entitiesArr = entities.ToArray();

        if (!entitiesArr.Any())
            return Enumerable.Empty<Link>();

        var rslts = new List<Link>();

        foreach (var entity in entitiesArr)
        {
            var link = Map(entity);

            if (link != null)
                rslts.Add(link);
        }

        return rslts;
    }

    internal static LinkTagEntity? Map(LinkTag? tag)
    {
        if (tag is null)
            return null;

        var entity = new LinkTagEntity(tag.Name, tag.Count);

        return entity;
    }

    internal static IEnumerable<LinkTagEntity> Map(IEnumerable<LinkTag>? tags)
    {
        if (tags == null)
            return Enumerable.Empty<LinkTagEntity>();

        var tagsArr = tags.ToArray();

        if (!tagsArr.Any())
            return Enumerable.Empty<LinkTagEntity>();

        var rslts = new List<LinkTagEntity>();

        foreach (var tag in tagsArr)
        {
            var entity = Map(tag);

            if (entity != null)
                rslts.Add(entity);
        }

        return rslts;
    }

    internal static LinkTag? Map(LinkTagEntity? entity)
    {
        if (entity == null)
            return null;

        var model = new LinkTag(entity.Name, entity.Count, entity.Weight);

        return model;
    }

    /// <summary>
    /// Maps a collection of Link Tag Entities to a collection of Link Tag Models
    /// </summary>
    /// <param name="entities">The entities to be mapped from</param>
    /// <returns>
    /// If entities is null or empty, then an empty IEnumerable of LinkTag is returned.
    /// Else a collection of Link Tags
    /// </returns>
    internal static IEnumerable<LinkTag> Map(IEnumerable<LinkTagEntity>? entities)
    {
        if (entities == null)
            return Enumerable.Empty<LinkTag>();

        entities = entities.ToList();

        if (!entities.Any())
            return Enumerable.Empty<LinkTag>();

        var rslts = new List<LinkTag>();

        foreach (var entity in entities)
        {
            var tag = Map(entity);

            if (tag != null)
                rslts.Add(tag);
        }

        return rslts;
    }


}
