using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using BlogProject.Models;
using System.Threading.Tasks;

namespace BlogProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

       
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var user = new ApplicationUser { UserName = model.UserName, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "User"); // Назначаем базовую роль "User"
                return Ok(new { message = "User registered successfully" });
            }

            return BadRequest(result.Errors);
        }

        
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = _userManager.Users;
            return Ok(users);
        }

       
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound();

            return Ok(user);
        }

     
        [HttpPut("{id}")]
        public async Task<IActionResult> EditUser(string id, [FromBody] EditUserModel model)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound();

            user.UserName = model.UserName;
            user.Email = model.Email;

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
                return Ok(user);

            return BadRequest(result.Errors);
        }

        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound();

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
                return Ok(new { message = "User deleted successfully" });

            return BadRequest(result.Errors);
        }
    }

    public class RegisterModel
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class EditUserModel
    {
        public string UserName { get; set; }
        public string Email { get; set; }
    }
}
