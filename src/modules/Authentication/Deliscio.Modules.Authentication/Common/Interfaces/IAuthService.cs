using Deliscio.Core.Models;
using Deliscio.Modules.Authentication.Common.Models;
using Deliscio.Modules.Authentication.Common.Models.Requests;
using Deliscio.Modules.Authentication.Data.Entities;
using FluentResults;
using Microsoft.AspNetCore.Identity;

namespace Deliscio.Modules.Authentication.Common.Interfaces;

public interface IAuthService
{
    ValueTask<Result<bool>> AddUserToRoleAsync(string userId, string roleId);

    /// <summary>
    /// Creates a new user without signing them in
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    ValueTask<Result<User?>> CreateUserAsync(CreateAuthUserRequest request);

    /// <summary>
    /// Gets a Paged Results of Users
    /// </summary>
    /// <param name="pageNo">The number of the page of results to be returned</param>
    /// <param name="pageSize">The number of users to return with the page</param>
    /// <returns></returns>
    Result<PagedResults<User>> GetUsersAsync(int pageNo = 1, int pageSize = 50);

    Task<Result<User>> GetUserAsync(string id);

    Task<Result<User>> GetUserByEmailAsync(string email);

    Task<Result<User>> GetUserByNameAsync(string name);

    ValueTask<Result<AuthUserEntity?>> RegisterUserAsync(string username, string email, string password, bool isPersistent);


    Task<Result<SignInResult>> SignInAsync(LoginRequest request);

    Task SignOutAsync();

    ValueTask<Result<Role?>> CreateRoleAsync(string name);

    ValueTask<Result<Role?>> DeleteRoleAsync(string id, string name);

    Result<Role?> GetRoleAsync(string id);

    Result<Role?> GetRoleByNameAsync(string name);

    ValueTask<Result<Role[]>> GetRolesAsync();

    Result<Role[]> GetRolesAsync(string[] ids);
}