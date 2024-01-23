using Deliscio.Modules.Links.Common.Models;
using Deliscio.Modules.Links.Data.Entities;

namespace Deliscio.Modules.Links.Mappers;

internal static class Mapper
{
    private const string TYPE_NOT_SUPPORTED_ERROR = "Type {0} is not supported.";

    internal static LinkEntity? Map(Link? link)
    {
        if (link is null)
            return null;

        var entity = new LinkEntity(new Guid(link.Id), link.Url, link.Title, new Guid(link.SubmittedById))
        {
            Description = link.Description,
            Domain = link.Domain,
            ImageUrl = link.ImageUrl,
            Keywords = link.Keywords,
            //IsExcluded = link.IsExcluded,
            //IsFlagged = link.IsFlagged,
            Tags = Map(link.Tags).ToList(),
            Title = link.Title,

            DateCreated = link.DateCreated,
            DateUpdated = link.DateUpdated
        };

        return entity;
    }


    internal static Link? Map(LinkEntity? entity)
    {
        return entity == null ? null : Map<Link>(entity);
    }

    /// <summary>
    /// Gets an IEnumerable of Link from the collection of LinkEntities.
    /// If any of the converted LinkEntities ends up being null, then they are discarded from the final results.
    /// </summary>
    /// <param name="entities"></param>
    /// <returns>IEnumerable of non-nullable Link</returns>
    internal static IEnumerable<Link> Map(IEnumerable<LinkEntity>? entities)
    {
        return Map<Link>(entities);
    }

    internal static T? Map<T>(LinkEntity? entity)
    {
        if (typeof(T) != typeof(Link) && typeof(T) != typeof(LinkItem))
            throw new ArgumentException(string.Format(TYPE_NOT_SUPPORTED_ERROR, typeof(T).Name));

        if (entity is null)
            return default;

        Uri.TryCreate(entity.ImageUrl, UriKind.Absolute, out var imgUri);
        var imgUrl = imgUri?.OriginalString ?? string.Empty;

        if (!string.IsNullOrWhiteSpace(imgUrl))
        {
            //TODO: Do it better
            if (imgUrl.Contains("//images/nomad-hydra.png"))
            {
                imgUrl = imgUrl.Replace("//images/", "/images/");
            }
        }

        if (typeof(T) == typeof(Link))
        {
            var model = new Link(entity.Id, entity.Url, entity.SubmittedById)
            {
                Description = entity.Description,
                Domain = entity.Domain,
                ImageUrl = imgUrl, //entity.ImageUrl,
                //IsExcluded = entity.IsExcluded,
                //IsFlagged = entity.IsFlagged,
                Tags = Map(entity.Tags).ToList(),
                Title = entity.Title,

                DateCreated = entity.DateCreated,
                DateUpdated = entity.DateUpdated,
            };

            return (T)(object)model;
        }

        if (typeof(T) == typeof(LinkItem))
        {
            var model = new LinkItem(entity.Id, entity.Url, entity.Title, entity.Description, entity.Domain, entity.ImageUrl, Map(entity.Tags), entity.DateCreated);

            return (T)(object)model;
        }

        return default;
    }

    /// <summary>
    /// Gets an IEnumerable of T (where T is either Link or a LinkItem from the collection of LinkEntities.
    /// If any of the converted LinkEntities ends up being null, then they are discarded from the final results.
    /// </summary>
    /// <param name="entities">The collection of LinkEntities to be mapped to either Links or LinkItems</param>
    /// <returns>IEnumerable of non-nullable Link</returns>
    internal static IEnumerable<T> Map<T>(IEnumerable<LinkEntity>? entities)
    {
        if (entities == null)
            return Enumerable.Empty<T>();

        var entitiesArr = entities.ToArray();

        if (!entitiesArr.Any())
            return Enumerable.Empty<T>();

        var rslts = new List<T>();

        foreach (var entity in entitiesArr)
        {
            var link = Map<T>(entity);

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
