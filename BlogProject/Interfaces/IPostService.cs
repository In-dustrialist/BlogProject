using BlogProject.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using BlogProject.Interfaces; 
using BlogProject.Models;

namespace BlogProject.Interfaces
{
    public interface IPostService
    {
        Task<List<Post>> GetAllPostsAsync();
        Task<Post> GetPostByIdAsync(int id);
        Task<Post> CreatePostAsync(CreatePostViewModel model);
        Task<Post> UpdatePostAsync(int id, EditPostViewModel model);
        Task<bool> DeletePostAsync(int id);
    }
}
