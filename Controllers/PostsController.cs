using Microsoft.AspNetCore.Mvc;
using BlogProject.Models;
using BlogProject.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;

namespace BlogProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly BlogDbContext _context;

        public PostController(BlogDbContext context)
        {
            _context = context;
        }

        
        [HttpPost]
        public async Task<IActionResult> CreatePost([FromBody] Post model)
        {
            _context.Posts.Add(model);
            await _context.SaveChangesAsync();
            return Ok(model);
        }

        
        [HttpGet]
        public async Task<IActionResult> GetAllPosts()
        {
            var posts = await _context.Posts.ToListAsync();
            return Ok(posts);
        }

        
        [HttpGet("author/{authorId}")]
        public async Task<IActionResult> GetPostsByAuthor(string authorId)
        {
            var posts = await _context.Posts.Where(p => p.AuthorId == authorId).ToListAsync();
            return Ok(posts);
        }

        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPostById(int id)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post == null)
                return NotFound();

            return Ok(post);
        }

        
        [HttpPut("{id}")]
        public async Task<IActionResult> EditPost(int id, [FromBody] Post model)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post == null)
                return NotFound();

            post.Title = model.Title;
            post.Content = model.Content;

            await _context.SaveChangesAsync();
            return Ok(post);
        }

        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePost(int id)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post == null)
                return NotFound();

            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Post deleted successfully" });
        }
    }
}
