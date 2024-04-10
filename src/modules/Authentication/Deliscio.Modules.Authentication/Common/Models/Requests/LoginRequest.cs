using System.ComponentModel.DataAnnotations;

namespace Deliscio.Modules.Authentication.Common.Models.Requests;

public record LoginRequest
{
    [Required]
    public string Login { get; set; }

    [Required]
    public string Password { get; set; }

    public bool RememberMe { get; set; } = false;

    public LoginRequest(string login, string password, bool remember = false)
    {
        Login = login;
        Password = password;
        RememberMe = remember;
    }
}