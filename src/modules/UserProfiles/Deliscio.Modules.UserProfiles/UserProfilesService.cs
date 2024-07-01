using Ardalis.GuardClauses;
using Deliscio.Common.Abstracts;
using Deliscio.Core.Models;
using Deliscio.Modules.UserProfiles.Common.Errors;
using Deliscio.Modules.UserProfiles.Common.Interfaces;
using Deliscio.Modules.UserProfiles.Common.Models;
using Deliscio.Modules.UserProfiles.Common.Models.Requests;
using Deliscio.Modules.UserProfiles.Data;
using Deliscio.Modules.UserProfiles.Mappers;
using FluentResults;
using Microsoft.Extensions.Logging;
using Structurizr.Annotations;

namespace Deliscio.Modules.UserProfiles;

[Component(Description = "Deliscio service that deals with the User Profiles", Technology = "C#")]
[UsedBySoftwareSystem("Deliscio.Apis.WebApi", Description = "User Profiles Service")]

public sealed class UserProfilesService : ServiceBase, IUserProfilesService
{
    private readonly ILogger<UserProfilesService> _logger;
    private readonly IUserProfilesRepository _repository;

    //public UserProfilesService(MongoDbClient client, ILogger<UserProfilesService> logger)
    //{
    //    Guard.Against.Null(client);
    //    Guard.Against.Null(logger);

    //    _repository = new UserProfilesRepository(client);
    //    _logger = logger;
    //}

    public UserProfilesService(IUserProfilesRepository repository, ILogger<UserProfilesService> logger)
    {
        Guard.Against.Null(repository);
        Guard.Against.Null(logger);

        _repository = repository;
        _logger = logger;
    }

    public async Task<Result<UserProfile>> AddAsync(CreateUserProfileRequest request, CancellationToken token = default)
    {
        Guard.Against.Null(request);

        var entity = UserProfileEntity.Create(request.UserId, request.Email, request.DisplayName, request.DateRegistered);

        try
        {
            await _repository.AddAsync(entity, token);

            var userProfile = Mapper.Map(entity);

            if (userProfile is null)
                return Result.Fail(new UserProfileNotCreated());

            return Result.Ok(userProfile);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error adding user profile", entity);
            return Result.Fail(new UserProfileNotCreated() { Reasons = { new Error(e.Message) } });
        }
    }

    public async Task<Result<UserProfile?>> GetAsync(string userId, CancellationToken token = default)
    {
        Guard.Against.NullOrWhiteSpace(userId);

        var user = await _repository.GetAsync(userId, token);

        if (user is null)
            return Result.Fail(new UserProfileNotFound());

        return Result.Ok(Mapper.Map(user));
    }

    /// <summary>
    /// Searches for user's based on their display name and/or email address.
    /// If neither are provided, all users are returned.
    /// </summary>
    /// <param name="displayName">The user's display Name</param>
    /// <param name="email">The user's email address</param>
    /// <param name="pageNo">The number of the page of results.</param>
    /// <param name="pageSize">The size of the page - max number of items to return</param>
    /// <param name="token"></param>
    /// <returns>A PagedResults object of UserProfileItems</returns>
    public async Task<PagedResults<UserProfileItem>> SearchAsync(string displayName = "", string email = "", int pageNo = 1, int pageSize = 50, CancellationToken token = default)
    {
        Guard.Against.NegativeOrZero(pageNo);
        Guard.Against.NegativeOrZero(pageSize);

        var rslts = await _repository.SearchAsync(displayName, email, pageNo, pageSize, token);

        var items = Mapper.Map<UserProfileItem>(rslts.Results);

        return new PagedResults<UserProfileItem>(items, pageNo, pageNo, rslts.TotalCount, offset: 0);
    }
}
