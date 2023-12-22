using System.ComponentModel.DataAnnotations;

namespace Deliscio.Apis.WebApi.Common.Requests;

public sealed record RegisterRequest
{
    [Required]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string UserName { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;
}