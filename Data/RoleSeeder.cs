using Microsoft.AspNetCore.Identity;
using BlogProject.Models;
using System.Linq;
using System.Threading.Tasks;

namespace BlogProject.Data
{
    public static class RoleSeeder
    {
        public static async Task SeedRoles(RoleManager<Role> roleManager)
        {
            string[] roleNames = { "Admin", "Moderator", "User" };

            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    var role = new Role { Name = roleName };
                    await roleManager.CreateAsync(role);
                }
            }
        }
    }
}
