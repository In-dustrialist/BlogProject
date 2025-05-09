using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using BlogProject.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace BlogProject.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly RoleManager<Role> _roleManager;

        public AdminController(RoleManager<Role> roleManager)
        {
            _roleManager = roleManager;
        }

       
        public async Task<IActionResult> Roles()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            return View(roles);
        }

        
        public IActionResult CreateRole()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateRole(CreateRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var existingRole = await _roleManager.FindByNameAsync(model.Name);
                if (existingRole != null)
                {
                    ModelState.AddModelError(string.Empty, "Роль с таким названием уже существует.");
                    return View(model);
                }

                var role = new Role { Name = model.Name, Description = model.Description };
                var result = await _roleManager.CreateAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("Roles");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }
    }
}
