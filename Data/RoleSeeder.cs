using System.Linq;
using BlogProject.Models;

namespace BlogProject.Data
{
    public static class RoleSeeder
    {
        public static void SeedRoles(BlogDbContext context)
        {
            if (!context.Roles.Any())
            {
                context.Roles.AddRange(
                    new Role { Name = "Admin" },
                    new Role { Name = "Moderator" },
                    new Role { Name = "User" }
                );
                context.SaveChanges();
            }
        }
    }
}
