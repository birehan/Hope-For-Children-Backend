using Microsoft.AspNetCore.Identity;

namespace Domain
{
    public class AppUser : IdentityUser
    {
        public enum Role
        {
            SuperAdmin,
            Admin
        }

        public Role UserRole { get; set; }
    }
}