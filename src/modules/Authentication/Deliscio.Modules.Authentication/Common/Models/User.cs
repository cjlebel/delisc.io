namespace Deliscio.Modules.Authentication.Common.Models;


public record User
{
    public string Id { get; init; }

    public string Username { get; set; }

    public string Email { get; init; }

    public string[] Roles { get; set; } = [];

    public bool IsAdmin { get; set; } = false;

    public bool IsGuest { get; set; } = true;

    public bool IsLocked { get; set; } = false;

    public DateTime DateCreated { get; set; }

    public DateTime DateLastSeen { get; set; }
}