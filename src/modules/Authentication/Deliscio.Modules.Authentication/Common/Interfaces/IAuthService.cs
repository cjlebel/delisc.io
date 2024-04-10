using Deliscio.Core.Models;
using Deliscio.Modules.Authentication.Common.Models;
using Deliscio.Modules.Authentication.Common.Models.Requests;
using Deliscio.Modules.Authentication.Data.Entities;
using FluentResults;
using Microsoft.AspNetCore.Identity;

namespace Deliscio.Modules.Authentication.Common.Interfaces;

public interface IAuthService
{
    ValueTask<Result<bool>> UserAssignRole(string userId, string roleId);

    /// <summary>
    /// Creates a new user without signing them in
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    ValueTask<Result<User?>> UserCreateAsync(CreateUserRequest request);

    /// <summary>
    /// Gets a Paged Results of Users
    /// </summary>
    /// <param name="pageNo">The number of the page of results to be returned</param>
    /// <param name="pageSize">The number of users to return with the page</param>
    /// <returns></returns>
    Result<PagedResults<User>> UsersGet(int pageNo = 1, int pageSize = 50);

    Task<Result<User>> UsersGetByIdAsync(string id);

    Task<Result<User>> UsersGetByNameAsync(string name);

    ValueTask<Result<AuthUser?>> UserRegisterAsync(string username, string email, string password, bool isPersistent);


    Task<Result<SignInResult>> UserSignInAsync(LoginRequest request);

    Task UserSignOutAsync();

    ValueTask<Result<Role?>> RolesCreateAsync(string name);

    ValueTask<Result<Role?>> RolesDeleteAsync(string id, string name);

    ValueTask<Result<Role[]>> RolesGetAsync();

    Result<Role?> RolesGetById(string id);

    Result<Role?> RolesGetByName(string name);

    Result<Role[]> RolesGetByIds(string[] ids);
}