using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using BlogProject.Models;

namespace BlogProject.Data
{
    public class BlogDbContext : IdentityDbContext<ApplicationUser>
    {
        public BlogDbContext(DbContextOptions<BlogDbContext> options) : base(options) { }

        public DbSet<Post> Posts { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<PostTag> PostTags { get; set; }

        // Добавляем DbSet для IdentityRole
        public DbSet<IdentityRole> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            
            modelBuilder.Entity<PostTag>()
                .HasKey(pt => new { pt.PostId, pt.TagId });

            modelBuilder.Entity<PostTag>()
                .HasOne(pt => pt.Post)
                .WithMany(p => p.PostTags)
                .HasForeignKey(pt => pt.PostId);

            modelBuilder.Entity<PostTag>()
                .HasOne(pt => pt.Tag)
                .WithMany(t => t.PostTags)
                .HasForeignKey(pt => pt.TagId);

            
            modelBuilder.Entity<Post>()
                .HasOne(p => p.Author)
                .WithMany(u => u.Posts)
                .HasForeignKey(p => p.AuthorId)
                .IsRequired();

           
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Post)
                .WithMany(p => p.Comments)
                .HasForeignKey(c => c.PostId);

          
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Author)
                .WithMany()
                .HasForeignKey(c => c.AuthorId)
                .IsRequired();
        }
    }
}
