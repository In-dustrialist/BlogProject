using BlogProject.Models;
using Microsoft.AspNetCore.Identity;

namespace BlogProject.Interfaces
{
    public interface IRoleService
    {
        Task<List<Role>> GetAllRolesAsync();
        Task<Role> GetRoleByIdAsync(string id);
        Task<IdentityResult> CreateRoleAsync(CreateRoleViewModel model);
        Task<IdentityResult> UpdateRoleAsync(string id, CreateRoleViewModel model);
        Task<IdentityResult> DeleteRoleAsync(string id);
    }
}
