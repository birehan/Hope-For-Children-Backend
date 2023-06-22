using Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using static Domain.AppUser;

namespace Persistence
{
    public class Seed
    {
        public static async Task SeedData(DataContext context, UserManager<AppUser> userManager, IConfiguration configuration
            )
        {

            // Check if there are no users in the database
            if (!context.Users.Any())
            {
                // Read the admin user details from appsettings.json
                var adminUser = configuration.GetSection("AdminUser");

                var userName = adminUser["UserName"];
                var email = adminUser["Email"];
                var password = adminUser["Password"];
                var role = adminUser["Role"];


                // Create a new instance of the AppUser
                var user = new AppUser
                {
                    UserName = userName,
                    Email = email,
                    UserRole = role == Role.SuperAdmin.ToString() ? Role.SuperAdmin : Role.Admin
                };

                // Create the admin user with the UserManager
                var result = await userManager.CreateAsync(user, password);
            }


        }
    }
}

