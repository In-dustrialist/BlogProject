using BlogProject.Interfaces;
using BlogProject.Models;
using BlogProject.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlogProject.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentService _commentService;

        public CommentsController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        /// <summary>
        /// Получить все комментарии.
        /// </summary>
        /// <returns>Список всех комментариев.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Comment>>> GetAllComments()
        {
            var comments = await _commentService.GetAllCommentsAsync();
            return Ok(comments);
        }

        /// <summary>
        /// Получить комментарий по ID.
        /// </summary>
        /// <param name="id">ID комментария.</param>
        /// <returns>Комментарий с заданным ID.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Comment>> GetCommentById(int id)
        {
            var comment = await _commentService.GetCommentByIdAsync(id);
            if (comment == null)
            {
                return NotFound(); // Возвращаем 404, если комментарий не найден.
            }
            return Ok(comment); // Возвращаем комментарий с кодом 200 OK.
        }

        /// <summary>
        /// Создать новый комментарий.
        /// </summary>
        /// <param name="comment">Комментарий, который нужно создать.</param>
        /// <returns>Созданный комментарий с его ID.</returns>
        [HttpPost]
        public async Task<ActionResult<Comment>> CreateComment([FromBody] Comment comment)
        {
            if (comment == null)
            {
                return BadRequest("Неверные данные."); // Если комментарий null, возвращаем ошибку 400 Bad Request.
            }

            var createdComment = await _commentService.CreateCommentAsync(comment);
            return CreatedAtAction(nameof(GetCommentById), new { id = createdComment.Id }, createdComment); // Возвращаем созданный комментарий с кодом 201 Created.
        }

        /// <summary>
        /// Обновить комментарий.
        /// </summary>
        /// <param name="id">ID комментария, который нужно обновить.</param>
        /// <param name="comment">Обновленные данные комментария.</param>
        /// <returns>Статус операции.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateComment(int id, [FromBody] Comment comment)
        {
            if (id != comment.Id)
            {
                return BadRequest("ID комментария не совпадает."); // Если ID не совпадает, возвращаем ошибку 400.
            }

            var updatedComment = await _commentService.UpdateCommentAsync(comment);
            if (updatedComment == null)
            {
                return NotFound(); // Возвращаем ошибку 404, если комментарий не найден.
            }

            return NoContent(); // Возвращаем успешный статус 204 No Content после успешного обновления.
        }

        /// <summary>
        /// Удалить комментарий.
        /// </summary>
        /// <param name="id">ID комментария, который нужно удалить.</param>
        /// <returns>Статус операции.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComment(int id)
        {
            var result = await _commentService.DeleteCommentAsync(id);
            if (!result)
            {
                return NotFound(); // Если комментарий не найден, возвращаем ошибку 404.
            }

            return NoContent(); // Возвращаем статус 204 No Content после успешного удаления.
        }
    }
}
