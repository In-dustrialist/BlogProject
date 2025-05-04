using Microsoft.AspNetCore.Mvc;
using BlogProject.Models;
using BlogProject.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace BlogProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagController : ControllerBase
    {
        private readonly BlogDbContext _context;

        public TagController(BlogDbContext context)
        {
            _context = context;
        }

        
        [HttpPost]
        public async Task<IActionResult> CreateTag([FromBody] Tag model)
        {
            _context.Tags.Add(model);
            await _context.SaveChangesAsync();
            return Ok(model);
        }

        
        [HttpGet]
        public async Task<IActionResult> GetAllTags()
        {
            var tags = await _context.Tags.ToListAsync();
            return Ok(tags);
        }

        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTagById(int id)
        {
            var tag = await _context.Tags.FindAsync(id);
            if (tag == null)
                return NotFound();

            return Ok(tag);
        }

        
        [HttpPut("{id}")]
        public async Task<IActionResult> EditTag(int id, [FromBody] Tag model)
        {
            var tag = await _context.Tags.FindAsync(id);
            if (tag == null)
                return NotFound();

            tag.Name = model.Name;

            await _context.SaveChangesAsync();
            return Ok(tag);
        }

       
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTag(int id)
        {
            var tag = await _context.Tags.FindAsync(id);
            if (tag == null)
                return NotFound();

            _context.Tags.Remove(tag);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Tag deleted successfully" });
        }
    }
}
