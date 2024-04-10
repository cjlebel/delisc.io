namespace Deliscio.Modules.Authentication.Common.Models;

public record Role
{
    public string Id { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
}