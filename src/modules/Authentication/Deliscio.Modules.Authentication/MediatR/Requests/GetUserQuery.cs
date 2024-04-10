using Deliscio.Core.Models;
using Deliscio.Modules.Authentication.Common.Interfaces;
using Deliscio.Modules.Authentication.Common.Models;
using FluentResults;
using MediatR;

namespace Deliscio.Modules.Authentication.MediatR.Requests;

public sealed record GetUserQuery : IRequest<FluentResults.Result<User>>
{
    public string UserId { get; }

    public GetUserQuery(string userId)
    {
        UserId = userId;
    }
}

public class GetUserQueryHandler : IRequestHandler<GetUserQuery, FluentResults.Result<User>>
{
    private readonly IAuthService _authService;

    public GetUserQueryHandler(IAuthService authService)
    {
        _authService = authService;
    }


    public async Task<FluentResults.Result<User>> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.UserId))
            return Result.Fail("The userId is required");

        var user = await _authService.UsersGetByIdAsync(request.UserId);

        return user;
    }
}