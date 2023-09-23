using System.Collections.ObjectModel;
using Deliscio.Modules.Links.Common.Models;
using Deliscio.Modules.Links.Data.Entities;

namespace Deliscio.Modules.Links.Mappers;

public static class Mapper
{
    public static LinkEntity? Map(Link? link)
    {
        if (link is null)
            return null;

        var entity = new LinkEntity(new Guid(link.Id), link.Url, link.Title)
        {
            Description = link.Description,
            Domain = link.Domain,
            ImageUrl = link.ImageUrl,
            //IsExcluded = link.IsExcluded,
            IsFlagged = link.IsFlagged,
            Tags = Map(link.Tags).ToList(),
            Title = link.Title,

            DateCreated = link.DateCreated,
            DateUpdated = link.DateUpdated
        };

        return entity;
    }


    public static Link? Map(LinkEntity? entity)
    {
        if (entity == null)
            return null;

        var model = new Link(entity.Id, entity.Url, entity.SubmittedById)
        {
            Description = entity.Description,
            Domain = entity.Domain,
            ImageUrl = entity.ImageUrl,
            //IsExcluded = entity.IsExcluded,
            IsFlagged = entity.IsFlagged,
            Tags = new ReadOnlyCollection<LinkTag>(Map(entity.Tags).ToList()),
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
    public static IEnumerable<Link> Map(IEnumerable<LinkEntity>? entities)
    {
        if (entities == null)
            return Enumerable.Empty<Link>();

        entities = entities as LinkEntity[] ?? Array.Empty<LinkEntity>();

        if (!entities.Any())
            return Enumerable.Empty<Link>();

        //var rslts = entities.Select(entity => Map(entity)).Where(link => link != null).ToList();

        var rslts = new List<Link>();

        foreach (var entity in entities)
        {
            var link = Map(entity);

            if (link != null)
                rslts.Add(link);
        }

        return rslts;
    }

    public static LinkTagEntity? Map(LinkTag? tag)
    {
        if (tag is null)
            return null;

        var entity = new LinkTagEntity(tag.Name, tag.Count);

        return entity;
    }

    public static IEnumerable<LinkTagEntity> Map(IEnumerable<LinkTag>? tags)
    {
        if (tags == null)
            return Enumerable.Empty<LinkTagEntity>();

        tags = tags as LinkTag[] ?? Array.Empty<LinkTag>();

        if (!tags.Any())
            return Enumerable.Empty<LinkTagEntity>();

        var rslts = new List<LinkTagEntity>();

        foreach (var tag in tags)
        {
            var entity = Map(tag);

            if (entity != null)
                rslts.Add(entity);
        }

        return rslts;
    }

    public static LinkTag? Map(LinkTagEntity? entity)
    {
        if (entity == null)
            return null;

        var model = new LinkTag(entity.Name, entity.Count);

        return model;
    }

    public static IEnumerable<LinkTag> Map(IEnumerable<LinkTagEntity>? entities)
    {
        if (entities == null)
            return Enumerable.Empty<LinkTag>();

        entities = entities as LinkTagEntity[] ?? Array.Empty<LinkTagEntity>();

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
