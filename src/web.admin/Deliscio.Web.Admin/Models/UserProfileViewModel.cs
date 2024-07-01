using Deliscio.Modules.Authentication.Common.Models;
using Deliscio.Modules.UserProfiles.Common.Models;

namespace Deliscio.Web.Admin.Models;

public class UserProfileViewModel
{
    public User User { get; set; }

    public UserProfile UserProfile { get; set; }

    public Role[] Roles { get; set; } = [];

    public bool IsAdmin => Roles.Any(r => r.Name == "Admin");

    public UserProfileViewModel(User user, UserProfile userProfile, Role[] roles)
    {
        User = user;
        UserProfile = userProfile;
        Roles = roles;
    }
}