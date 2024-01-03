using Deliscio.Modules.Authentication.Common.Interfaces;
using Deliscio.Modules.Authentication.Common.Models;
using MediatR;

namespace Deliscio.Modules.Authentication.MediatR.Commands.Handlers;

public class SignInCommandHandler : IRequestHandler<SignInCommand, (bool IsSuccess, string Message, AuthUser? user)>
{
    private readonly IAuthService _authService;

    public SignInCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<(bool IsSuccess, string Message, AuthUser? user)> Handle(SignInCommand request, CancellationToken cancellationToken)
    {
        return await _authService.SignInAsync(request.EmailOrUsername, request.Password);
    }
}