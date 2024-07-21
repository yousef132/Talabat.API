using Microsoft.AspNetCore.Identity;

namespace Talabat.Core.Entities.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public string DisplayName { get; set; }

        public UserAddress? Address { get; set; }

    }
}
