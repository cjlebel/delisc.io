namespace Deliscio.Apis.WebApi.Common.Requests;

public class SignInRequest
{
    public string EmailOrUserName { get; set; }
    public string Password { get; set; }
}