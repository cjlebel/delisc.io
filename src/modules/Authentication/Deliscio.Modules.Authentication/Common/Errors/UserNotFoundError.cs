using FluentResults;

namespace Deliscio.Modules.Authentication.Common.Errors;

public class UserNotFoundError : IError
{
    public string Message { get; }
    public Dictionary<string, object> Metadata { get; }
    public List<IError> Reasons { get; }
}