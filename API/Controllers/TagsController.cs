using BlogProject.Interfaces;
using BlogProject.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Controllers
{
    /// <summary>
    /// Контроллер для управления тегами.
    /// Позволяет получать, создавать, обновлять и удалять теги.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TagsController : ControllerBase
    {
        private readonly ITagService _tagService;

        public TagsController(ITagService tagService)
        {
            _tagService = tagService;
        }

        /// <summary>
        /// Получить все теги.
        /// </summary>
        /// <returns>Список всех тегов.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Tag>), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetAllTags()
        {
            var tags = await _tagService.GetAllTagsAsync();
            if (tags == null || tags.Count == 0)
            {
                return NotFound();
            }
            return Ok(tags);
        }

        /// <summary>
        /// Получить тег по ID.
        /// </summary>
        /// <param name="id">ID тега.</param>
        /// <returns>Информация о теге.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Tag), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetTag(int id)
        {
            var tag = await _tagService.GetTagByIdAsync(id);
            if (tag == null)
            {
                return NotFound();
            }

            return Ok(tag);
        }

        /// <summary>
        /// Создать новый тег.
        /// </summary>
        /// <param name="tag">Модель тега для создания.</param>
        /// <returns>Созданный тег.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(Tag), 201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateTag([FromBody] Tag tag)
        {
            if (tag == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdTag = await _tagService.CreateTagAsync(tag);
            return CreatedAtAction(nameof(GetTag), new { id = createdTag.Id }, createdTag);
        }

        /// <summary>
        /// Обновить существующий тег.
        /// </summary>
        /// <param name="id">ID тега для обновления.</param>
        /// <param name="tag">Модель тега для обновления.</param>
        /// <returns>Обновленный тег.</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Tag), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateTag(int id, [FromBody] Tag tag)
        {
            if (tag == null || id != tag.Id)
            {
                return BadRequest();
            }

            var updatedTag = await _tagService.UpdateTagAsync(id, tag);
            if (updatedTag == null)
            {
                return NotFound();
            }

            return Ok(updatedTag);
        }

        /// <summary>
        /// Удалить тег по ID.
        /// </summary>
        /// <param name="id">ID тега для удаления.</param>
        /// <returns>Результат операции удаления.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteTag(int id)
        {
            var isDeleted = await _tagService.DeleteTagAsync(id);
            if (!isDeleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
