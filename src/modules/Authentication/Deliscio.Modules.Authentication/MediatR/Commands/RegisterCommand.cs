using MediatR;

namespace Deliscio.Modules.Authentication.MediatR.Commands;

public sealed record RegisterCommand : IRequest<(bool IsSuccess, string Message, string[] ErrorMessages)>
{
    public string Email { get; }

    public string Password { get; }

    public string Username { get; }

    public RegisterCommand(string email, string password, string username)
    {
        Email = email;
        Password = password;
        Username = username;
    }
}