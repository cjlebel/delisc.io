using System.ComponentModel.DataAnnotations;

namespace Deliscio.Modules.Authentication.Common.Models.Requests;

public record RegisterUserRequest
{
    [Required]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }

    [Required]
    public string Username { get; set; }

    public bool IsPersistent { get; set; }

    public RegisterUserRequest() { }

    public RegisterUserRequest(string email, string password, string username)
    {
        Email = email;
        Password = password;
        Username = username;
    }
}