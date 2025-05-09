using Microsoft.AspNetCore.Mvc;

namespace BlogProject.Controllers
{
    public class ErrorController : Controller
    {
        [Route("Error/403")]
        public IActionResult AccessDenied()
        {
            return View("403");
        }

        [Route("Error/404")]
        public IActionResult NotFoundPage()
        {
            return View("404");
        }

        [Route("Error/500")]
        public IActionResult ServerError()
        {
            return View("500");
        }
    }
}
