using Microsoft.AspNetCore.Mvc;
using BlogProject.Models;
using NLog;
using System.Diagnostics;

namespace BlogProject.Controllers
{
    public class HomeController : Controller
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public IActionResult Index()
        {
            logger.Info("Пользователь зашел на главную страницу.");
            return View();
        }

        public IActionResult Privacy()
        {
            logger.Info("Пользователь открыл страницу Privacy.");
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            var requestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
            logger.Error($"Произошла ошибка: RequestId = {requestId}");
            return View(new ErrorViewModel { RequestId = requestId });
        }
    }
}
