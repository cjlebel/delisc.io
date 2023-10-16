using AspNetCore.Identity.MongoDbCore.Models;

namespace Deliscio.Modules.Authentication.Common.Models;

public class AuthUser : MongoIdentityUser<Guid> { }