using Deliscio.Modules.Links.Application.Dtos;
using Deliscio.Modules.Links.Infrastructure.Data.Entities;
using MongoDB.Bson;

namespace Deliscio.Modules.Links.Mappers;

internal static class Mapper
{
    private const string TYPE_NOT_SUPPORTED_ERROR = "Type {0} is not supported.";

    ////internal static LinkEntity? Map(LinkDto? link)
    ////{
    ////    if (link is null)
    ////        return null;

    ////    var entity = new LinkEntity()
    ////    {
    ////        LinkId = ObjectId.Parse(link.LinkId),
    ////        Description = link.Description,
    ////        Domain = link.Domain,
    ////        ImageUrl = link.ImageUrl,
    ////        IsActive = link.IsActive,
    ////        IsFlagged = link.IsFlagged,
    ////        Keywords = link.Keywords,
    ////        TotalLikes = link.TotalLikes,
    ////        //IsExcluded = link.IsExcluded,

    ////        TagCollection = Map(link.TagCollection).ToArray(),
    ////        Title = link.Title.Trim(),
    ////        Url = link.Url,

    ////        DateCreated = link.DateCreated,
    ////        CreatedById = ObjectId.Parse(link.CreatedByUserId),

    ////        DateUpdated = link.DateUpdated == DateTimeOffset.MinValue ? null : link.DateUpdated,
    ////        UpdatedById = ObjectId.Parse(link.UpdatedByUserId),

    ////        DateDeleted = link.DateDeleted == DateTimeOffset.MinValue ? null : link.DateDeleted,
    ////    };

    ////    return entity;
    ////}


    //////internal static LinkDto? Map(LinkEntity? entity)
    //////{
    //////    return entity == null ? null : Map<LinkDto>(entity);
    //////}

    /////// <summary>
    /////// Gets an IEnumerable of Link from the collection of LinkEntities.
    /////// If any of the converted LinkEntities ends up being null, then they are discarded from the final results.
    /////// </summary>
    /////// <param name="entities"></param>
    /////// <returns>IEnumerable of non-nullable Link</returns>
    ////internal static IEnumerable<LinkDto> Map(IEnumerable<LinkEntity>? entities)
    ////{
    ////    return Map<LinkDto>(entities);
    ////}

    ////internal static T? Map<T>(LinkEntity? entity)
    ////{
    ////    if (typeof(T) != typeof(LinkDto) && typeof(T) != typeof(LinkItemDto))
    ////        throw new ArgumentException(string.Format(TYPE_NOT_SUPPORTED_ERROR, typeof(T).Name));

    ////    if (entity is null)
    ////        return default;

    ////    Uri.TryCreate(entity.ImageUrl, UriKind.Absolute, out var imgUri);
    ////    var imgUrl = imgUri?.OriginalString ?? string.Empty;

    ////    if (!string.IsNullOrWhiteSpace(imgUrl))
    ////    {
    ////        //TODO: Do it better
    ////        if (imgUrl.Contains("//images/nomad-hydra.png"))
    ////        {
    ////            imgUrl = imgUrl.Replace("//images/", "/images/");
    ////        }
    ////    }

    ////    if (typeof(T) == typeof(LinkDto))
    ////    {
    ////        var model = new LinkDto
    ////        {
    ////            LinkId = entity.LinkId.ToString(),
    ////            Url = entity.Url,

    ////            Description = entity.Description,
    ////            Domain = entity.Domain,
    ////            ImageUrl = imgUrl,

    ////            IsActive = entity.IsActive,
    ////            IsFlagged = entity.IsFlagged,

    ////            TotalLikes = entity.TotalLikes,
    ////            TotalSaves = entity.TotalSaves,

    ////            TagCollection = Map(entity.TagCollection).ToList(),
    ////            Title = entity.Title.Trim(),

    ////            DateCreated = entity.DateCreated,
    ////            CreatedByUserId = entity.CreatedById.ToString(),

    ////            DateUpdated = entity.DateUpdated,
    ////            UpdatedByUserId = entity.UpdatedById.ToString(),

    ////            DateDeleted = entity.DateDeleted,
    ////            DeletedByUserId = entity.DeletedById.ToString()
    ////        };

    ////        return (T)(object)model;
    ////    }

    ////    if (typeof(T) == typeof(LinkItemDto))
    ////    {
    ////        var model = new LinkItemDto(entity.LinkId.ToString(), entity.Url, entity.Title, entity.Description, entity.Domain, entity.ImageUrl, Map(entity.TagCollection), entity.DateCreated, entity.DateUpdated)
    ////        {
    ////            IsActive = entity.IsActive,
    ////            IsDeleted = entity.IsDeleted,
    ////            IsFlagged = entity.IsFlagged,

    ////            Likes = entity.TotalLikes,
    ////            Saves = entity.TotalSaves
    ////        };

    ////        return (T)(object)model;
    ////    }

    ////    return default;
    ////}

    /////// <summary>
    /////// Gets an IEnumerable of T (where T is either Link or a LinkItem from the collection of LinkEntities.
    /////// If any of the converted LinkEntities ends up being null, then they are discarded from the final results.
    /////// </summary>
    /////// <param name="entities">The collection of LinkEntities to be mapped to either Links or LinkItems</param>
    /////// <returns>IEnumerable of non-nullable Link</returns>
    ////internal static IEnumerable<T> Map<T>(IEnumerable<LinkEntity>? entities) where T : class
    ////{
    ////    if (entities == null)
    ////        return Enumerable.Empty<T>();

    ////    var entitiesArr = entities.ToArray();

    ////    if (!entitiesArr.Any())
    ////        return Enumerable.Empty<T>();

    ////    var rslts = new List<T>();

    ////    foreach (var entity in entitiesArr)
    ////    {
    ////        var link = Map<T>(entity);

    ////        if (link != null)
    ////            rslts.Add(link);
    ////    }

    ////    return rslts;
    ////}

    //internal static LinkTagEntity? Map(LinkTagDto? tag)
    //{
    //    if (tag is null)
    //        return null;

    //    var entity = new LinkTagEntity(tag.Name, tag.Count, tag.Weight);

    //    return entity;
    //}

    //internal static IEnumerable<LinkTagEntity> Map(IEnumerable<LinkTagDto>? tags)
    //{
    //    if (tags == null)
    //        return Enumerable.Empty<LinkTagEntity>();

    //    var tagsArr = tags.ToArray();

    //    if (!tagsArr.Any())
    //        return Enumerable.Empty<LinkTagEntity>();

    //    var rslts = new List<LinkTagEntity>();

    //    foreach (var tag in tagsArr)
    //    {
    //        var entity = Map(tag);

    //        if (entity != null)
    //            rslts.Add(entity);
    //    }

    //    return rslts;
    //}

    //internal static LinkTagDto? Map(LinkTagEntity? entity)
    //{
    //    if (entity == null)
    //        return null;

    //    var model = new LinkTagDto(entity.Name, entity.Count, entity.Weight);

    //    return model;
    //}

    ///// <summary>
    ///// Maps a collection of Link Tag Entities to a collection of Link Tag Models
    ///// </summary>
    ///// <param name="entities">The entities to be mapped from</param>
    ///// <returns>
    ///// If entities is null or empty, then an empty IEnumerable of LinkTag is returned.
    ///// Else a collection of Link TagsCollection
    ///// </returns>
    //internal static IEnumerable<LinkTagDto> Map(IEnumerable<LinkTagEntity>? entities)
    //{
    //    if (entities == null)
    //        return Enumerable.Empty<LinkTagDto>();

    //    entities = entities.ToList();

    //    if (!entities.Any())
    //        return Enumerable.Empty<LinkTagDto>();

    //    var rslts = new List<LinkTagDto>();

    //    foreach (var entity in entities)
    //    {
    //        var tag = Map(entity);

    //        if (tag != null)
    //            rslts.Add(tag);
    //    }

    //    return rslts;
    //}
}
