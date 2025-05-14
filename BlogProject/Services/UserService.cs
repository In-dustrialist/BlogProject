using Microsoft.AspNetCore.Identity;
using BlogProject.Interfaces;
using BlogProject.Models;
using Microsoft.EntityFrameworkCore;

public class UserService : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<Role> _roleManager;

    public UserService(UserManager<ApplicationUser> userManager, RoleManager<Role> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<List<UserRolesViewModel>> GetAllUsersAsync()
    {
        var users = await _userManager.Users.ToListAsync();
        var result = new List<UserRolesViewModel>();

        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);
            result.Add(new UserRolesViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Roles = roles.ToList()
            });
        }

        return result;
    }

    public async Task<EditUserViewModel?> GetUserForEditAsync(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null) return null;

        var roles = await _userManager.GetRolesAsync(user);

        return new EditUserViewModel
        {
            Id = user.Id,
            Username = user.UserName,
            Email = user.Email,
            IsUser = roles.Contains("User"),
            IsAdmin = roles.Contains("Admin"),
            IsModerator = roles.Contains("Moderator")
        };
    }

    public async Task<bool> UpdateUserAsync(string id, EditUserViewModel model)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null) return false;

        user.UserName = model.Username;
        user.Email = model.Email;

        if (!string.IsNullOrWhiteSpace(model.Password))
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, model.Password);
            if (!result.Succeeded)
                return false;
        }

        var updateResult = await _userManager.UpdateAsync(user);
        if (!updateResult.Succeeded) return false;

        var currentRoles = await _userManager.GetRolesAsync(user);
        var newRoles = new List<string>();
        if (model.IsUser) newRoles.Add("User");
        if (model.IsAdmin) newRoles.Add("Admin");
        if (model.IsModerator) newRoles.Add("Moderator");

        await _userManager.RemoveFromRolesAsync(user, currentRoles);
        await _userManager.AddToRolesAsync(user, newRoles);

        return true;
    }
}
