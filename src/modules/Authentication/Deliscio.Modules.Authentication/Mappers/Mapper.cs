using System.Data;
using Deliscio.Modules.Authentication.Common.Models;
using Deliscio.Modules.Authentication.Data.Entities;

namespace Deliscio.Modules.Authentication.Mappers;

public static class Mapper
{
    public static User? Map(AuthUserEntity? authUser)
    {
        if (authUser is null)
            return default;

        return new User
        {
            Id = authUser.Id.ToString(),
            Username = authUser.UserName ?? string.Empty,
            Email = authUser.Email ?? string.Empty,
            Roles = [],
            IsAdmin = false,
            IsGuest = false,
            IsLocked = false,
            DateCreated = authUser.CreatedOn
        };
    }

    public static IEnumerable<User> Map(IEnumerable<AuthUserEntity>? authUsers)
    {
        var arr = authUsers?.ToArray() ?? Array.Empty<AuthUserEntity>();

        if (!arr.Any())
            yield break;

        foreach (var user in arr)
        {
            var rslt = Map(user);

            if (rslt is not null)
                yield return rslt;
        }
    }

    public static Role? Map(AuthRoleEntity? authRole)
    {
        if (authRole is null)
            return default;

        return new Role
        {
            Id = authRole.Id.ToString(),
            Name = authRole.Name ?? string.Empty,
        };
    }

    public static IEnumerable<Role> Map(IEnumerable<AuthRoleEntity>? authRoles)
    {
        var arr = authRoles?.ToArray() ?? Array.Empty<AuthRoleEntity>();

        if (!arr.Any())
            yield break;

        foreach (var role in arr)
        {
            var rslt = Map(role);

            if (rslt is not null)
                yield return rslt;
        }
    }
}