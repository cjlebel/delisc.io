using Deliscio.Modules.Authentication.Common.Models;
using MediatR;

namespace Deliscio.Modules.Authentication.MediatR.Commands;

public sealed record SignInCommand : IRequest<(bool IsSuccess, string Message, AuthUser? User)>
{
    public string EmailOrUsername { get; }

    public string Password { get; }

    public SignInCommand(string emailOrUsername, string password)
    {
        EmailOrUsername = emailOrUsername;
        Password = password;
    }
}