using Deliscio.Modules.UserProfiles.Common.Interfaces;
using Deliscio.Modules.UserProfiles.Common.Models;
using Deliscio.Modules.UserProfiles.Common.Models.Requests;
using MediatR;

namespace Deliscio.Modules.UserProfiles.MediatR.Commands;

public sealed record CreateUserProfileCommand : IRequest<FluentResults.Result<UserProfile>>
{
    public CreateUserProfileRequest Request { get; }

    public CreateUserProfileCommand(CreateUserProfileRequest request)
    {
        Request = request;
    }
}

public class CreateUserProfileCommandHandler : IRequestHandler<CreateUserProfileCommand, FluentResults.Result<UserProfile>>
{
    private readonly IUserProfilesService _profilesService;

    public CreateUserProfileCommandHandler(IUserProfilesService profilesService)
    {
        _profilesService = profilesService;
    }

    public async Task<FluentResults.Result<UserProfile>> Handle(CreateUserProfileCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;

        var result = await _profilesService.AddAsync(request, cancellationToken);

        return result;
    }
}