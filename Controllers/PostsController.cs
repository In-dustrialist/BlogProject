using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BlogProject.Models;
using BlogProject.Data;
using System.Threading.Tasks;
using System.Linq;
using System;
using System.Collections.Generic;

namespace BlogProject.Controllers
{
    public class PostsController : Controller
    {
        private readonly BlogDbContext _context;

        public PostsController(BlogDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var posts = await _context.Posts
                .Include(p => p.PostTags)
                .ThenInclude(pt => pt.Tag)
                .ToListAsync();

            return View(posts);
        }

        public async Task<IActionResult> All()
        {
            var posts = await _context.Posts
                .Include(p => p.PostTags)
                .ThenInclude(pt => pt.Tag)
                .ToListAsync();

            return View(posts);
        }

        public async Task<IActionResult> Create()
        {
            var tags = await _context.Tags.ToListAsync();

            var viewModel = new CreatePostViewModel
            {
                AvailableTags = tags
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreatePostViewModel model)
        {
            if (ModelState.IsValid)
            {
                var post = new Post
                {
                    Title = model.Title,
                    Summary = model.Summary,
                    Content = model.Content,
                    CreatedAt = DateTime.Now,
                    AuthorId = User.Identity.Name,
                    ViewCount = 0
                };

                if (model.SelectedTags != null)
                {
                    foreach (var tagId in model.SelectedTags)
                    {
                        post.PostTags.Add(new PostTag { TagId = tagId });
                    }
                }

                _context.Posts.Add(post);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            model.AvailableTags = await _context.Tags.ToListAsync();
            return View(model);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var post = await _context.Posts
                .Include(p => p.PostTags)
                .ThenInclude(pt => pt.Tag)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (post == null) return NotFound();

            post.ViewCount++;
            _context.Update(post);
            await _context.SaveChangesAsync();

            var comments = await _context.Comments
                .Where(c => c.PostId == post.Id)
                .Include(c => c.Author)
                .ToListAsync();

            var viewModel = new PostDetailsViewModel
            {
                Post = post,
                Comments = comments,
                NewComment = new Comment { PostId = post.Id }
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddComment(PostDetailsViewModel model)
        {
            if (ModelState.IsValid)
            {
                var comment = new Comment
                {
                    Content = model.NewComment.Content,
                    AuthorId = User.Identity.Name,
                    PostId = model.NewComment.PostId
                };

                _context.Comments.Add(comment);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Details), new { id = model.Post.Id });
            }

            return View(nameof(Details), model);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var post = await _context.Posts
                .Include(p => p.PostTags)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (post == null) return NotFound();

            var model = new EditPostViewModel
            {
                Id = post.Id,
                Title = post.Title,
                Summary = post.Summary,
                Content = post.Content,
                SelectedTags = post.PostTags.Select(pt => pt.TagId).ToList(),
                AvailableTags = await _context.Tags.ToListAsync()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditPostViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.AvailableTags = await _context.Tags.ToListAsync();
                return View(model);
            }

            var post = await _context.Posts
                .Include(p => p.PostTags)
                .FirstOrDefaultAsync(p => p.Id == model.Id);

            if (post == null) return NotFound();

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

            return RedirectToAction(nameof(Index));
        }
    }
}
