using Deliscio.Modules.Authentication.Common.Interfaces;
using Deliscio.Modules.Authentication.Data.Entities;
using MediatR;

namespace Deliscio.Modules.Authentication.MediatR.Commands;

public sealed record RegisterUserCommand : IRequest<FluentResults.Result<AuthUser?>>
{
    public string Email { get; }

    public string Password { get; }

    public string Username { get; }

    public bool IsPersistent { get; set; }

    public RegisterUserCommand(string email, string password, string username, bool isPersistent = false)
    {
        Email = email;
        Password = password;
        Username = username;
        IsPersistent = isPersistent;
    }
}

public class RegisterCommandHandler : IRequestHandler<RegisterUserCommand, FluentResults.Result<AuthUser?>>
{
    private readonly IAuthService _authService;

    public RegisterCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<FluentResults.Result<AuthUser?>> Handle(RegisterUserCommand command, CancellationToken cancellationToken)
    {
        return await _authService.UserRegisterAsync(command.Username, command.Email, command.Password, command.IsPersistent);
    }
}