using System;
using System.Collections.Generic;
using BlogProject.Models;

namespace BlogProject.Models
{
    public class CreatePostViewModel
    {
        public string Title { get; set; }
        public string Summary { get; set; }
        public string Content { get; set; }

        
        public IEnumerable<Tag> AvailableTags { get; set; }

        
        public List<int> SelectedTags { get; set; } = new List<int>();
    }
}
