using Deliscio.Core.Models;
using Deliscio.Modules.UserProfiles.Common.Models;
using Deliscio.Modules.UserProfiles.Common.Models.Requests;
using FluentResults;

namespace Deliscio.Modules.UserProfiles.Common.Interfaces;

public interface IUserProfilesService
{
    Task<Result<UserProfile>> AddAsync(CreateUserProfileRequest request, CancellationToken token = default);

    Task<Result<UserProfile?>> GetAsync(string userId, CancellationToken token = default);

    Task<PagedResults<UserProfileItem>> SearchAsync(string displayName = "", string email = "", int page = 1, int pageSize = 50, CancellationToken token = default);
}