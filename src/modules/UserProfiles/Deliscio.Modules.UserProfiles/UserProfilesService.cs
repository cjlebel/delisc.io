using Ardalis.GuardClauses;
using Deliscio.Common.Abstracts;
using Deliscio.Core.Data.Mongo;
using Deliscio.Core.Models;
using Deliscio.Modules.UserProfiles.Common.Interfaces;
using Deliscio.Modules.UserProfiles.Common.Models;
using Deliscio.Modules.UserProfiles.Data;
using Deliscio.Modules.UserProfiles.Mappers;
using FluentResults;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
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

    public UserProfilesService(IOptions<MongoDbOptions> options, ILogger<UserProfilesService> logger)
    {
        Guard.Against.Null(options);
        Guard.Against.Null(logger);

        _repository = new UserProfilesRepository(options);
        _logger = logger;
    }

    public async Task<string> AddAsync(UserProfile userProfile, CancellationToken token = default)
    {
        Guard.Against.Null(userProfile);

        var entity = UserProfileEntity.Create(Guid.Parse(userProfile.Id), userProfile.Email, userProfile.DisplayName);

        try
        {
            await _repository.AddAsync(entity, token);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error adding user profile", userProfile);
            throw;
        }

        return entity.Id.ToString();
    }

    public async Task<Results<UserProfile?>> GetAsync(Guid userId, CancellationToken token = default)
    {
        Guard.Against.Default(userId);

        var user = await _repository.GetAsync(userId, token);

        if (user is null)
            return Results<UserProfile?>.Fail("User not found");

        return Results<UserProfile?>.Ok(Mapper.Map(user));
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

        return new PagedResults<UserProfileItem>(items, pageNo, pageNo, rslts.TotalCount);
    }
}
