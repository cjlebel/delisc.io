using Deliscio.Apis.WebApi.Common.Requests;
using Deliscio.Apis.WebApi.Common.Responses;

namespace Deliscio.Apis.WebApi.Common.Interfaces;

public interface IUsersManager
{
    Task<(bool IsSuccess, string Message, string[] ErrorMessages)> RegisterAsync(RegisterRequest request);


    Task<SignInResponse> SignInAsync(SignInRequest request);

    Task SignOutAsync();
}