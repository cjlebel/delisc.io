using Ardalis.GuardClauses;
using Deliscio.Core.Data.Mongo;
using Deliscio.Modules.UserProfiles.Common.Interfaces;
using Deliscio.Modules.UserProfiles.Common.Models;
using Deliscio.Modules.UserProfiles.Data;
using Deliscio.Modules.UserProfiles.Mappers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Structurizr.Annotations;

namespace Deliscio.Modules.UserProfiles;

[Component(Description = "Deliscio service that deals with the User Profiles", Technology = "C#")]
[UsedBySoftwareSystem("Deliscio.Apis.WebApi", Description = "User Profiles Service")]

public sealed class UserProfilesService : IUserProfilesService
{
    private readonly ILogger<UserProfilesService> _logger;
    private readonly IUserProfilesRepository _repository;

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

    public async Task<UserProfile?> GetAsync(Guid userId, CancellationToken token = default)
    {
        Guard.Against.Default(userId);

        var user = await _repository.GetAync(userId, token);

        if (user is null)
            return null;

        return Mapper.Map(user);
    }
}
