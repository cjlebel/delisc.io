using System.ComponentModel.DataAnnotations;

namespace Deliscio.Modules.Authentication.Common.Models.Requests;

public record CreateAuthUserRequest
{
    [Required]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }

    [Required]
    public string Username { get; set; }

    public string[] RoleIds { get; set; }

    public CreateAuthUserRequest() { }

    public CreateAuthUserRequest(string email, string password, string username, string[] roleIds)
    {
        Email = email;
        Password = password;
        Username = username;
        RoleIds = roleIds ?? [];
    }
}