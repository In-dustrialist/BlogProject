using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using BlogProject.Models;
using System.Threading.Tasks;

namespace BlogProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<ApplicationUser> userManager,
                                 SignInManager<ApplicationUser> signInManager,
                                 RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
        {
            if (model.Password != model.ConfirmPassword)
            {
                return BadRequest(new { message = "Passwords do not match" });
            }

            var user = new ApplicationUser { UserName = model.Username, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                
                await _userManager.AddToRoleAsync(user, "User");
                return Ok(new { message = "User registered successfully" });
            }

            return BadRequest(result.Errors);
        }

        
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return Unauthorized(new { message = "Invalid credentials" });

            var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
            if (result.Succeeded)
            {
                
                return Ok(new { message = "Login successful" });
            }

            return Unauthorized(new { message = "Invalid credentials" });
        }

        
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok(new { message = "Logged out successfully" });
        }
    }

    public class RegisterViewModel
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }  // Подтверждение пароля
    }

    public class LoginModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
