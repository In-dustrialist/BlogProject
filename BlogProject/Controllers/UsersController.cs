using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using BlogProject.Models;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using NLog;

namespace BlogProject.Controllers
{
    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public UsersController(UserManager<ApplicationUser> userManager, RoleManager<Role> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            logger.Info("Пользователь открыл страницу списка всех пользователей.");

            var users = await _userManager.Users.ToListAsync();
            var userRolesList = new List<UserRolesViewModel>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userRolesList.Add(new UserRolesViewModel
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    Roles = roles.ToList()
                });
            }

            return View(userRolesList);
        }

        [HttpGet("Users/Edit/{id}")]
        public async Task<IActionResult> Edit(string id)
        {
            logger.Info($"Пользователь открыл страницу редактирования пользователя с ID: {id}.");

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                logger.Warn($"Пользователь с ID: {id} не найден.");
                return NotFound();
            }

            var roles = await _userManager.GetRolesAsync(user);

            var model = new EditUserViewModel
            {
                Id = user.Id,
                Username = user.UserName,
                Email = user.Email,
                IsUser = roles.Contains("User"),
                IsAdmin = roles.Contains("Admin"),
                IsModerator = roles.Contains("Moderator")
            };

            return View(model);
        }

        [HttpPost("Users/Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, EditUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                logger.Warn($"Ошибка валидации при редактировании пользователя с ID: {id}.");
                return View(model);
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                logger.Warn($"Пользователь с ID: {id} не найден для редактирования.");
                return NotFound();
            }

            user.UserName = model.Username;
            user.Email = model.Email;

            if (!string.IsNullOrWhiteSpace(model.Password))
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var result = await _userManager.ResetPasswordAsync(user, token, model.Password);
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        logger.Error($"Ошибка смены пароля для пользователя с ID: {id}. Ошибка: {error.Description}");
                        ModelState.AddModelError(string.Empty, error.Description);
                    }

                    return View(model);
                }

                logger.Info($"Пароль для пользователя с ID: {id} был успешно изменен.");
            }

            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                foreach (var error in updateResult.Errors)
                {
                    logger.Error($"Ошибка при обновлении пользователя с ID: {id}. Ошибка: {error.Description}");
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                return View(model);
            }

            if (User.IsInRole("Admin"))
            {
                var currentRoles = await _userManager.GetRolesAsync(user);
                var newRoles = new List<string>();
                if (model.IsUser) newRoles.Add("User");
                if (model.IsAdmin) newRoles.Add("Admin");
                if (model.IsModerator) newRoles.Add("Moderator");

                await _userManager.RemoveFromRolesAsync(user, currentRoles);
                await _userManager.AddToRolesAsync(user, newRoles);

                logger.Info($"Роли пользователя с ID: {id} были обновлены.");
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
