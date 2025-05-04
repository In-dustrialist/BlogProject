using Microsoft.AspNetCore.Mvc;
using BlogProject.Models;
using BlogProject.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace BlogProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly BlogDbContext _context;

        public CommentController(BlogDbContext context)
        {
            _context = context;
        }

       
        [HttpPost]
        public async Task<IActionResult> CreateComment([FromBody] Comment model)
        {
            _context.Comments.Add(model);
            await _context.SaveChangesAsync();
            return Ok(model);
        }

        
        [HttpGet]
        public async Task<IActionResult> GetAllComments()
        {
            var comments = await _context.Comments.ToListAsync();
            return Ok(comments);
        }

       
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCommentById(int id)
        {
            var comment = await _context.Comments.FindAsync(id);
            if (comment == null)
                return NotFound();

            return Ok(comment);
        }

        
        [HttpPut("{id}")]
        public async Task<IActionResult> EditComment(int id, [FromBody] Comment model)
        {
            var comment = await _context.Comments.FindAsync(id);
            if (comment == null)
                return NotFound();

            comment.Content = model.Content;

            await _context.SaveChangesAsync();
            return Ok(comment);
        }

        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComment(int id)
        {
            var comment = await _context.Comments.FindAsync(id);
            if (comment == null)
                return NotFound();

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Comment deleted successfully" });
        }
    }
}
