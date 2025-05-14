using Microsoft.AspNetCore.Identity;
using BlogProject.Models;
using System.Threading.Tasks;

namespace BlogProject.Data
{
    public class UserSeeder
    {
        public static async Task SeedUsers(UserManager<ApplicationUser> userManager, RoleManager<Role> roleManager)
        {
            await CreateRoleIfNotExists(roleManager, "Admin");
            await CreateRoleIfNotExists(roleManager, "Moderator");
            await CreateRoleIfNotExists(roleManager, "User");

            if (await userManager.FindByEmailAsync("admin@example.com") == null)
            {
                var adminUser = new ApplicationUser
                {
                    UserName = "admin@example.com",
                    Email = "admin@example.com"
                };
                var result = await userManager.CreateAsync(adminUser, "Admin@123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }

            if (await userManager.FindByEmailAsync("moderator@example.com") == null)
            {
                var modUser = new ApplicationUser
                {
                    UserName = "moderator@example.com",
                    Email = "moderator@example.com"
                };
                var result = await userManager.CreateAsync(modUser, "Moderator@123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(modUser, "Moderator");
                }
            }

            if (await userManager.FindByEmailAsync("user@example.com") == null)
            {
                var user = new ApplicationUser
                {
                    UserName = "user@example.com",
                    Email = "user@example.com"
                };
                var result = await userManager.CreateAsync(user, "User@123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "User");
                }
            }
        }

        private static async Task CreateRoleIfNotExists(RoleManager<Role> roleManager, string roleName)
        {
            var role = await roleManager.FindByNameAsync(roleName);
            if (role == null)
            {
                await roleManager.CreateAsync(new Role { Name = roleName });
            }
        }
    }
}
