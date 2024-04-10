using Deliscio.Modules.UserProfiles.Common.Models;
using Deliscio.Modules.UserProfiles.Data;

namespace Deliscio.Modules.UserProfiles.Mappers;

internal static class Mapper
{
    private const string TYPE_NOT_SUPPORTED_ERROR = "Type {0} is not supported.";

    internal static UserProfileEntity? Map(UserProfile? userProfile)
    {
        if (userProfile is null)
            return default;

        return new UserProfileEntity(Guid.Parse(userProfile.Id), userProfile.Email, userProfile.DisplayName)
        {
            FirstName = userProfile.FirstName,
            ImageUrl = userProfile.ImageUrl,
            LastName = userProfile.LastName,
            Location = userProfile.Location
        };
    }

    internal static UserProfile? Map(UserProfileEntity? entity)
    {
        if (entity is null)
            return default;

        return new UserProfile(entity.Id.ToString(), entity.Email, entity.DisplayName)
        {
            FirstName = entity.FirstName,
            ImageUrl = entity.ImageUrl,
            LastName = entity.LastName,
            Location = entity.Location
        };
    }

    internal static T? Map<T>(UserProfileEntity? entity)
    {
        if (typeof(T) != typeof(UserProfile) && typeof(T) != typeof(UserProfileItem))
            throw new ArgumentException(string.Format(TYPE_NOT_SUPPORTED_ERROR, typeof(T).Name));

        if (entity is null)
            return default;

        if (typeof(T) == typeof(UserProfile))
        {
            var model = Map(entity);

            if (model != null)
                return (T)(object)model;
        }

        if (typeof(T) == typeof(UserProfileItem))
        {
            var model = new UserProfileItem
            {
                Id = entity.Id.ToString(),
                DisplayName = entity.DisplayName,
                IsActivated = true,
                IsOnline = true,
                Roles = Array.Empty<string>(),
                DateLastSeen = entity.DateLastSeen,
            };

            return (T)(object)model;
        }

        return default;
    }

    internal static IEnumerable<T> Map<T>(IEnumerable<UserProfileEntity>? entities)
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
}