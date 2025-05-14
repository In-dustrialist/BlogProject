using Microsoft.AspNetCore.Mvc;
using BlogProject.Models;
using BlogProject.Data;
using Microsoft.EntityFrameworkCore;
using NLog;
using System.Threading.Tasks;
using System.Linq;

namespace BlogProject.Controllers
{
    public class CommentsController : Controller
    {
        private readonly BlogDbContext _context;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public CommentsController(BlogDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var comments = await _context.Comments
                .Include(c => c.Author)
                .Include(c => c.Post)
                .ToListAsync();

            Logger.Info("Пользователь {0} просматривает список комментариев.", User.Identity.Name);
            return View(comments);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Comment model)
        {
            if (ModelState.IsValid)
            {
                _context.Comments.Add(model);
                await _context.SaveChangesAsync();
                Logger.Info("Пользователь {0} создал новый комментарий с Id {1}.", User.Identity.Name, model.Id);
                return RedirectToAction(nameof(Index));
            }

            Logger.Warn("Пользователь {0} попытался создать комментарий, но модель не прошла валидацию.", User.Identity.Name);
            return View(model);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var comment = await _context.Comments.FindAsync(id);
            if (comment != null)
            {
                _context.Comments.Remove(comment);
                await _context.SaveChangesAsync();
                Logger.Info("Пользователь {0} удалил комментарий с Id {1}.", User.Identity.Name, id);
            }
            else
            {
                Logger.Warn("Пользователь {0} попытался удалить несуществующий комментарий с Id {1}.", User.Identity.Name, id);
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var comment = await _context.Comments.FindAsync(id);
            if (comment == null)
            {
                Logger.Warn("Пользователь {0} попытался редактировать несуществующий комментарий с Id {1}.", User.Identity.Name, id);
                return NotFound();
            }
            return View(comment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Comment model)
        {
            if (id != model.Id)
            {
                Logger.Warn("Пользователь {0} попытался редактировать комментарий с неверным Id {1}.", User.Identity.Name, id);
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Comments.Update(model);
                await _context.SaveChangesAsync();
                Logger.Info("Пользователь {0} отредактировал комментарий с Id {1}.", User.Identity.Name, model.Id);
                return RedirectToAction(nameof(Index));
            }

            Logger.Warn("Пользователь {0} попытался отредактировать комментарий с Id {1}, но модель не прошла валидацию.", User.Identity.Name, model.Id);
            return View(model);
        }

        public async Task<IActionResult> Details(int id)
        {
            var comment = await _context.Comments
                .Include(c => c.Author)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (comment == null)
            {
                Logger.Warn("Пользователь {0} попытался просмотреть детали несуществующего комментария с Id {1}.", User.Identity.Name, id);
                return NotFound();
            }

            Logger.Info("Пользователь {0} просматривает детали комментария с Id {1}.", User.Identity.Name, id);
            return View(comment);
        }
    }
}
