using Deliscio.Modules.Authentication.Common.Models;

namespace Deliscio.Modules.Authentication.Common.Interfaces;

public interface IAuthService
{
    Task<IEnumerable<AuthUser>> GetUsers();

    Task<(bool IsSuccess, string Message, string[] ErrorMessages)> RegisterAsync(string username, string email, string password);


    Task<(bool IsSuccess, string Message, AuthUser? user)> SignInAsync(string emailOrUserName, string password);

    Task SignOutAsync();
}