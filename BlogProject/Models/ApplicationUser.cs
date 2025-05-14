using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace BlogProject.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ICollection<Post> Posts { get; set; } = new List<Post>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}
