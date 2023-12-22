using Deliscio.Modules.Authentication.Common.Interfaces;

using MediatR;

namespace Deliscio.Modules.Authentication.MediatR.Commands.Handlers;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, (bool IsSuccess, string Message, string[] ErrorMessages)>
{
    private readonly IAuthService _authService;

    public RegisterCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public Task<(bool IsSuccess, string Message, string[] ErrorMessages)> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        return _authService.RegisterAsync(request.Username, request.Email, request.Password);
    }
}