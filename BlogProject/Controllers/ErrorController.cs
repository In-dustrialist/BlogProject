using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BlogProject.Controllers
{
    public class ErrorController : Controller
    {
        private readonly ILogger<ErrorController> _logger;

        public ErrorController(ILogger<ErrorController> logger)
        {
            _logger = logger;
        }

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
            var exceptionDetails = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            if (exceptionDetails?.Error != null)
            {
                _logger.LogError(exceptionDetails.Error, "Произошла необработанная ошибка: {Message}", exceptionDetails.Error.Message);
            }
            else
            {
                _logger.LogError("Произошла необработанная ошибка, но деталей нет.");
            }

            return View("500");
        }
    }
}
