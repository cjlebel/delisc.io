using Deliscio.Apis.WebApi.Common.Requests;
using Deliscio.Apis.WebApi.Common.Responses;
using Deliscio.Modules.Authentication.Data.Entities;

namespace Deliscio.Apis.WebApi.Common.Interfaces;

public interface IUsersManager
{
    Task<FluentResults.Result<AuthUser?>> RegisterAsync(RegisterRequest request);


    Task<SignInResponse> SignInAsync(SignInRequest request);

    Task SignOutAsync();
}