using Amazon.Runtime.Internal;
using Deliscio.Modules.Authentication.Common.Interfaces;
using Deliscio.Modules.Authentication.Common.Models;
using Deliscio.Modules.Authentication.Common.Models.Requests;
using Deliscio.Modules.Authentication.Data.Entities;
using MediatR;

namespace Deliscio.Modules.Authentication.MediatR.Commands;

public sealed record CreateUserCommand : IRequest<FluentResults.Result<User?>>
{
    public CreateUserRequest Request { get; }

    public CreateUserCommand(CreateUserRequest request)
    {
        Request = request;
    }
}

public class CreateCommandHandler : IRequestHandler<CreateUserCommand, FluentResults.Result<User?>>
{
    private readonly IAuthService _authService;

    public CreateCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<FluentResults.Result<User?>> Handle(CreateUserCommand command, CancellationToken cancellationToken)
    {
        return await _authService.UserCreateAsync(command.Request);
    }
}