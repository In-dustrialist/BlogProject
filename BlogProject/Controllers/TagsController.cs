using Microsoft.AspNetCore.Mvc;
using BlogProject.Data;
using BlogProject.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NLog;

namespace BlogProject.Controllers
{
    public class TagsController : Controller
    {
        private readonly BlogDbContext _context;
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public TagsController(BlogDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            logger.Info("Пользователь открыл страницу со всеми тегами.");

            var tags = await _context.Tags.ToListAsync();
            return View(tags);
        }

        public IActionResult Create()
        {
            logger.Info("Пользователь открыл страницу для создания нового тега.");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Tag tag)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tag);
                await _context.SaveChangesAsync();

                logger.Info($"Тег '{tag.Name}' был успешно создан.");
                return RedirectToAction(nameof(Index));
            }

            logger.Warn("Ошибка при создании тега. Некорректные данные.");
            return View(tag);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                logger.Warn("Попытка редактирования несуществующего тега. ID = null");
                return NotFound();
            }

            var tag = await _context.Tags.FindAsync(id);
            if (tag == null)
            {
                logger.Warn($"Попытка редактирования несуществующего тега с ID: {id}");
                return NotFound();
            }

            logger.Info($"Пользователь открыл страницу редактирования тега с ID: {id}.");
            return View(tag);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Tag tag)
        {
            if (id != tag.Id)
            {
                logger.Warn($"Попытка редактирования тега с некорректным ID: {id}.");
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tag);
                    await _context.SaveChangesAsync();
                    logger.Info($"Тег с ID: {id} был успешно отредактирован.");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Tags.Any(e => e.Id == tag.Id))
                    {
                        logger.Error($"Ошибка при редактировании тега с ID: {id}. Тег не найден.");
                        return NotFound();
                    }
                    else
                    {
                        logger.Error($"Неизвестная ошибка при редактировании тега с ID: {id}.");
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            logger.Warn("Ошибка при редактировании тега. Некорректные данные.");
            return View(tag);
        }
    }
}
