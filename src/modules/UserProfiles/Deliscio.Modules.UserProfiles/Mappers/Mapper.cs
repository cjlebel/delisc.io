using Deliscio.Modules.UserProfiles.Common.Models;
using Deliscio.Modules.UserProfiles.Data;

namespace Deliscio.Modules.UserProfiles.Mappers;

internal static class Mapper
{
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
}