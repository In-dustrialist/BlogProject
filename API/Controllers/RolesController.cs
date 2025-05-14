using BlogProject.Interfaces;
using BlogProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Controllers
{
    /// <summary>
    /// Контроллер для управления ролями пользователей.
    /// Позволяет получать, создавать, обновлять и удалять роли.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RolesController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        /// <summary>
        /// Получить все роли.
        /// </summary>
        /// <returns>Список всех ролей.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Role>), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetAllRoles()
        {
            var roles = await _roleService.GetAllRolesAsync();
            if (roles == null || roles.Count == 0)
            {
                return NotFound();
            }
            return Ok(roles);
        }

        /// <summary>
        /// Получить роль по ID.
        /// </summary>
        /// <param name="id">ID роли.</param>
        /// <returns>Информация о роли.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Role), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetRoleById(string id)
        {
            var role = await _roleService.GetRoleByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }

            return Ok(role);
        }

        /// <summary>
        /// Создать новую роль.
        /// </summary>
        /// <param name="model">Модель для создания роли.</param>
        /// <returns>Созданная роль.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(Role), 201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateRole([FromBody] CreateRoleViewModel model)
        {
            if (model == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _roleService.CreateRoleAsync(model);
            if (result.Succeeded)
            {
                return CreatedAtAction(nameof(GetRoleById), new { id = model.Name }, model);
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return BadRequest(ModelState);
        }

        /// <summary>
        /// Обновить существующую роль.
        /// </summary>
        /// <param name="id">ID роли для обновления.</param>
        /// <param name="model">Модель для обновления роли.</param>
        /// <returns>Обновленная роль.</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Role), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateRole(string id, [FromBody] CreateRoleViewModel model)
        {
            if (model == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _roleService.UpdateRoleAsync(id, model);
            if (!result.Succeeded)
            {
                return NotFound();
            }

            return Ok(model);
        }

        /// <summary>
        /// Удалить роль по ID.
        /// </summary>
        /// <param name="id">ID роли для удаления.</param>
        /// <returns>Результат операции удаления.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteRole(string id)
        {
            var result = await _roleService.DeleteRoleAsync(id);
            if (!result.Succeeded)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
