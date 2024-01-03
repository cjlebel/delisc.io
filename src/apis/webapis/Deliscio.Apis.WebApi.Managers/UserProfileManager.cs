using Ardalis.GuardClauses;
using Deliscio.Apis.WebApi.Common.Abstracts;
using Deliscio.Apis.WebApi.Common.Interfaces;
using Deliscio.Modules.UserProfiles.Common.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Deliscio.Apis.WebApi.Managers;

public class UserProfileManager : ManagerBase<UserProfileManager>, IUserProfileManager
{
    private readonly IMediator _mediator;

    public UserProfileManager(IMediator mediator, ILogger<UserProfileManager> logger) : base(logger)
    {
        Guard.Against.Null(mediator);
        Guard.Against.Null(logger);

        _mediator = mediator;
    }

    public Task<UserProfile?> GetAsync(string userId, CancellationToken token = default)
    {
        Guard.Against.NullOrWhiteSpace(userId);

        throw new NotImplementedException();
    }
}