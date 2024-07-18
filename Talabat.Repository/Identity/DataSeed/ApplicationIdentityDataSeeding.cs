using Microsoft.AspNetCore.Identity;
using Talabat.Core.Entities.Identity;

namespace Talabat.Infrastructure.Identity.DataSeed
{
    public static class ApplicationIdentityDataSeeding
    {
        public static async Task SeedUserAsync(UserManager<ApplicationUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                var user = new ApplicationUser()
                {
                    DisplayName = "Yousef Saad",
                    Email = "YousefSaad@gmail.com",
                    UserName = "Yousef.Saad",
                    PhoneNumber = "012589265545"
                };

                await userManager.CreateAsync(user, "Aa@123.123");
            }



        }

    }
}
