using System.ComponentModel.DataAnnotations;

namespace Deliscio.Modules.Authentication.Common.Models.Requests;

public record CreateUserRequest
{
    [Required]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }

    [Required]
    public string Username { get; set; }

    public string[] RoleIds { get; set; }

    public CreateUserRequest() { }

    public CreateUserRequest(string email, string password, string username, string[] roleIds)
    {
        Email = email;
        Password = password;
        Username = username;
        RoleIds = roleIds ?? [];
    }
}