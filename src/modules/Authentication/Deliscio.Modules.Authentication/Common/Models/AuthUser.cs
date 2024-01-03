using System.ComponentModel.DataAnnotations.Schema;
using AspNetCore.Identity.MongoDbCore.Models;
using Deliscio.Core.Data.Mongo.Attributes;

namespace Deliscio.Modules.Authentication.Common.Models;

[Table("AuthUser")]
[BsonCollection("Users")]
public class AuthUser : MongoIdentityUser<Guid> { }