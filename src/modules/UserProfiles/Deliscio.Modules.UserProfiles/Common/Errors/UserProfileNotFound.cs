using FluentResults;

namespace Deliscio.Modules.UserProfiles.Common.Errors;

public class UserProfileNotFound : IError
{
    public string Message { get; }
    public Dictionary<string, object> Metadata { get; }
    public List<IError> Reasons { get; }
}