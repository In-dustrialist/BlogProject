using BlogProject.Models;

namespace BlogProject.Interfaces
{
    public interface IUserService
    {
        Task<List<UserRolesViewModel>> GetAllUsersAsync();
        Task<EditUserViewModel?> GetUserForEditAsync(string id);
        Task<bool> UpdateUserAsync(string id, EditUserViewModel model);
    }
}
