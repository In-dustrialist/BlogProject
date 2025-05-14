using System;
using System.Collections.Generic;

namespace BlogProject.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Summary { get; set; }
        public DateTime CreatedAt { get; set; }
        public string AuthorId { get; set; }
        public ApplicationUser Author { get; set; }

        public int ViewCount { get; set; } = 0;

        
        public ICollection<PostTag> PostTags { get; set; } = new List<PostTag>();

        
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}
