using Ardalis.GuardClauses;
using Deliscio.Modules.UserProfiles.Common.Interfaces;
using Deliscio.Modules.UserProfiles.Common.Models;
using MediatR;

namespace Deliscio.Modules.UserProfiles.MediatR.Queries;

public class GetUserProfileQuery : IRequest<FluentResults.Result<UserProfile?>>
{
    public string UserId { get; init; }

    public GetUserProfileQuery(int pageNo = 1, int pageSize = 50, bool? isOnline = null)
    {
        Guard.Against.NegativeOrZero(pageNo);
        Guard.Against.NegativeOrZero(pageSize);
    }

    public GetUserProfileQuery(string userId)
    {
        Guard.Against.NullOrEmpty(userId);

        UserId = userId;
    }
}

public class GetUserProfileQueryHandler : IRequestHandler<GetUserProfileQuery, FluentResults.Result<UserProfile?>>
{
    private readonly IUserProfilesService _service;

    public GetUserProfileQueryHandler(IUserProfilesService service)
    {
        Guard.Against.Null(service);

        _service = service;
    }

    public async Task<FluentResults.Result<UserProfile?>> Handle(GetUserProfileQuery command, CancellationToken cancellationToken)
    {
        var result = await _service.GetAsync(command.UserId, token: cancellationToken);

        if (result.IsError)
            return FluentResults.Result.Fail("User could not be found");

        return default;
    }
}