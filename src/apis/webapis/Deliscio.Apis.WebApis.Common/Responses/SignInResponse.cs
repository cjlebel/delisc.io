namespace Deliscio.Apis.WebApi.Common.Responses;

public sealed record SignInResponse
{
    public bool IsSuccess { get; set; } = false;

    public string Message { get; set; } = "Login failed";

    public UserInfo? User { get; set; }

    public class UserInfo
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string[] Roles { get; set; }

        public UserInfo(string id, string userName, string email, string[] roles)
        {
            Id = id;
            Email = email;
            Roles = roles;
            UserName = userName;
        }
    }
}