using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using BlogProject.Models;
using System.Threading.Tasks;
using System.Linq;
using NLog;

namespace BlogProject.Controllers
{
    public class RoleController : Controller
    {
        private readonly RoleManager<Role> _roleManager;
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public RoleController(RoleManager<Role> roleManager)
        {
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            logger.Info("Пользователь открыл страницу с ролями.");

            var roles = _roleManager.Roles.ToList();
            return View(roles);
        }

        public IActionResult Create()
        {
            logger.Info("Пользователь открыл страницу создания новой роли.");

            return View(new CreateRoleViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateRoleViewModel model)
        {
            if (!ModelState.IsValid)
            {
                logger.Warn("Ошибка при создании роли. Некорректные данные.");
                return View(model);
            }

            var role = new Role
            {
                Name = model.Name,
                Description = model.Description
            };

            var result = await _roleManager.CreateAsync(role);
            if (result.Succeeded)
            {
                logger.Info($"Роль '{role.Name}' была успешно создана.");
                return RedirectToAction(nameof(Index));
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);

            logger.Error($"Ошибка при создании роли '{role.Name}': {string.Join(", ", result.Errors.Select(e => e.Description))}");

            return View(model);
        }

        public async Task<IActionResult> Edit(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                logger.Warn($"Попытка редактирования несуществующей роли с ID: {id}");
                return NotFound();
            }

            var model = new CreateRoleViewModel
            {
                Name = role.Name,
                Description = role.Description
            };

            ViewBag.RoleId = role.Id;
            logger.Info($"Пользователь открыл страницу редактирования роли '{role.Name}' с ID: {id}.");

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, CreateRoleViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.RoleId = id;
                logger.Warn("Ошибка при редактировании роли. Некорректные данные.");
                return View(model);
            }

            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                logger.Warn($"Попытка редактирования несуществующей роли с ID: {id}");
                return NotFound();
            }

            role.Name = model.Name;
            role.Description = model.Description;

            var result = await _roleManager.UpdateAsync(role);
            if (result.Succeeded)
            {
                logger.Info($"Роль с ID: {id} была успешно отредактирована.");
                return RedirectToAction(nameof(Index));
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);

            logger.Error($"Ошибка при редактировании роли с ID: {id}: {string.Join(", ", result.Errors.Select(e => e.Description))}");

            ViewBag.RoleId = id;
            return View(model);
        }
    }
}
