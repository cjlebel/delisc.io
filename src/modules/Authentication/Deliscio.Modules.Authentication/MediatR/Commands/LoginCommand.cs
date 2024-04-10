using Deliscio.Modules.Authentication.Common.Interfaces;
using Deliscio.Modules.Authentication.Common.Models.Requests;
using Deliscio.Modules.Authentication.Data.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Deliscio.Modules.Authentication.MediatR.Commands;

public sealed record LoginCommand : IRequest<FluentResults.Result<SignInResult>>
{
    public LoginRequest Request { get; }

    public LoginCommand(string userName, string password, bool rememberMe = false)
    {
        Request = new LoginRequest(userName, password, rememberMe);
    }
}

public class LoginCommandHandler : IRequestHandler<LoginCommand, FluentResults.Result<SignInResult>>
{
    private readonly IAuthService _authService;

    public LoginCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<FluentResults.Result<SignInResult>> Handle(LoginCommand command, CancellationToken cancellationToken)
    {
        return await _authService.UserSignInAsync(command.Request);
    }
}