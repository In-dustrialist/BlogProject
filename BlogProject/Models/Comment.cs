namespace BlogProject.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Content { get; set; }

        public string AuthorId { get; set; }
        public ApplicationUser Author { get; set; }

        public int PostId { get; set; }
        public Post Post { get; set; }
    }
}
