using BlogProject.Models;
using BlogProject.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using BlogProject.Interfaces;  // Здесь указываем правильное пространство имён
using BlogProject.Models;

namespace API.Controllers
{
    /// <summary>
    /// Контроллер для управления постами.
    /// Позволяет получать, создавать, редактировать и удалять посты.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly IPostService _postService;

        public PostsController(IPostService postService)
        {
            _postService = postService;
        }

        /// <summary>
        /// Получить все посты.
        /// </summary>
        /// <returns>Список всех постов.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Post>), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetAllPosts()
        {
            var posts = await _postService.GetAllPostsAsync();
            if (posts == null || posts.Count == 0)
            {
                return NotFound();
            }

            return Ok(posts);
        }

        /// <summary>
        /// Получить пост по ID.
        /// </summary>
        /// <param name="id">ID поста.</param>
        /// <returns>Информация о посте.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Post), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetPost(int id)
        {
            var post = await _postService.GetPostByIdAsync(id);
            if (post == null)
            {
                return NotFound();
            }

            return Ok(post);
        }

        /// <summary>
        /// Создать новый пост.
        /// </summary>
        /// <param name="model">Модель для создания поста.</param>
        /// <returns>Созданный пост.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(Post), 201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreatePost([FromBody] CreatePostViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var post = await _postService.CreatePostAsync(model);
            return CreatedAtAction(nameof(GetPost), new { id = post.Id }, post);
        }

        /// <summary>
        /// Редактировать существующий пост.
        /// </summary>
        /// <param name="id">ID поста для редактирования.</param>
        /// <param name="model">Модель для обновления поста.</param>
        /// <returns>Обновленный пост.</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Post), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdatePost(int id, [FromBody] EditPostViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updatedPost = await _postService.UpdatePostAsync(id, model);
            if (updatedPost == null)
            {
                return NotFound();
            }

            return Ok(updatedPost);
        }

        /// <summary>
        /// Удалить пост.
        /// </summary>
        /// <param name="id">ID поста для удаления.</param>
        /// <returns>Результат операции удаления.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeletePost(int id)
        {
            var isDeleted = await _postService.DeletePostAsync(id);
            if (!isDeleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
