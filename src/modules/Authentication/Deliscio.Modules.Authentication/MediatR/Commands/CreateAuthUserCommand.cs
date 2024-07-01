using Deliscio.Modules.Authentication.Common.Interfaces;
using Deliscio.Modules.Authentication.Common.Models;
using Deliscio.Modules.Authentication.Common.Models.Requests;
using MediatR;

namespace Deliscio.Modules.Authentication.MediatR.Commands;

public sealed record CreateAuthUserCommand : IRequest<FluentResults.Result<User?>>
{
    public CreateAuthUserRequest Request { get; }

    public CreateAuthUserCommand(CreateAuthUserRequest request)
    {
        Request = request;
    }
}

public class CreateUserCommandHandler : IRequestHandler<CreateAuthUserCommand, FluentResults.Result<User?>>
{
    private readonly IAuthService _authService;

    public CreateUserCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<FluentResults.Result<User?>> Handle(CreateAuthUserCommand command, CancellationToken cancellationToken)
    {
        return await _authService.CreateUserAsync(command.Request);
    }
}