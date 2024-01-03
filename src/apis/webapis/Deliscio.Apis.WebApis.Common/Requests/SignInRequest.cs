using System.ComponentModel.DataAnnotations;

namespace Deliscio.Apis.WebApi.Common.Requests;

public sealed record SignInRequest
{
    [Required]
    public string EmailOrUserName { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;
}