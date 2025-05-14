namespace BlogProject.Models
{
    public class PostDetailsViewModel
    {
        public Post Post { get; set; }
        public List<Comment> Comments { get; set; }
        public Comment NewComment { get; set; }
    }
}
