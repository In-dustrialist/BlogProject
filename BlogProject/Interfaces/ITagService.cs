using BlogProject.Models;

namespace BlogProject.Interfaces
{
    public interface ITagService
    {
        Task<List<Tag>> GetAllTagsAsync();
        Task<Tag> GetTagByIdAsync(int id);
        Task<Tag> CreateTagAsync(Tag tag);
        Task<Tag> UpdateTagAsync(int id, Tag tag);
        Task<bool> DeleteTagAsync(int id);
    }
}
