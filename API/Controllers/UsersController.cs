using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using BlogProject.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NLog;
using System.Linq;
using System.Collections.Generic;

namespace API.Controllers
{
    /// <summary>
    /// Контроллер для управления пользователями.
    /// Позволяет получать информацию о пользователях, редактировать их данные и роли.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public UsersController(UserManager<ApplicationUser> userManager, RoleManager<Role> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        /// <summary>
        /// Получить список всех пользователей.
        /// </summary>
        /// <returns>Список пользователей с ролями.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<UserRolesViewModel>), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetUsers()
        {
            logger.Info("Пользователь запросил список всех пользователей.");

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

            return Ok(userRolesList);
        }

        /// <summary>
        /// Получить пользователя по ID.
        /// </summary>
        /// <param name="id">ID пользователя.</param>
        /// <returns>Информация о пользователе с ролями.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(UserRolesViewModel), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                logger.Warn($"Пользователь с ID: {id} не найден.");
                return NotFound();
            }

            var roles = await _userManager.GetRolesAsync(user);
            var model = new UserRolesViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Roles = roles.ToList()
            };

            return Ok(model);
        }

        /// <summary>
        /// Редактировать информацию о пользователе.
        /// </summary>
        /// <param name="id">ID пользователя для редактирования.</param>
        /// <param name="model">Модель для редактирования.</param>
        /// <returns>Статус операции.</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApplicationUser), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Edit(string id, [FromBody] EditUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                logger.Warn($"Ошибка валидации при редактировании пользователя с ID: {id}.");
                return BadRequest(ModelState);
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

                    return BadRequest(ModelState);
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

                return BadRequest(ModelState);
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

            return Ok(user);
        }
    }
}
