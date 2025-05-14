using BlogProject.Data;
using BlogProject.Interfaces;
using BlogProject.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogProject.Services
{
    public class PostService : IPostService
    {
        private readonly BlogDbContext _context;

        public PostService(BlogDbContext context)
        {
            _context = context;
        }

        public async Task<List<Post>> GetAllPostsAsync()
        {
            return await _context.Posts
                .Include(p => p.PostTags)
                .ThenInclude(pt => pt.Tag)
                .ToListAsync();
        }

        public async Task<Post> GetPostByIdAsync(int id)
        {
            return await _context.Posts
                .Include(p => p.PostTags)
                .ThenInclude(pt => pt.Tag)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Post> CreatePostAsync(CreatePostViewModel model)
        {
            var post = new Post
            {
                Title = model.Title,
                Summary = model.Summary,
                Content = model.Content,
                CreatedAt = DateTime.Now,
                AuthorId = "defaultAuthor", 
                ViewCount = 0,
                PostTags = model.SelectedTags?.Select(tagId => new PostTag { TagId = tagId }).ToList() ?? new List<PostTag>()
            };

            _context.Posts.Add(post);
            await _context.SaveChangesAsync();

            return post;
        }

        public async Task<Post> UpdatePostAsync(int id, EditPostViewModel model)
        {
            var post = await _context.Posts
                .Include(p => p.PostTags)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (post == null)
            {
                return null;
            }

            post.Title = model.Title;
            post.Summary = model.Summary;
            post.Content = model.Content;

            post.PostTags.Clear();
            if (model.SelectedTags != null)
            {
                foreach (var tagId in model.SelectedTags)
                {
                    post.PostTags.Add(new PostTag { TagId = tagId });
                }
            }

            await _context.SaveChangesAsync();

            return post;
        }

        public async Task<bool> DeletePostAsync(int id)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post == null)
            {
                return false;
            }

            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
