using Microsoft.EntityFrameworkCore;
using Personal_Blogging_Platform_API.Models;

namespace Personal_Blogging_Platform_API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<ArticleTag> ArticleTags { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>(b =>
            {
                b.HasKey(u => u.Id);
                b.HasIndex(u => u.Email).IsUnique();
                b.Property(u => u.Username).IsRequired().HasMaxLength(50);
                b.Property(u => u.Email).IsRequired().HasMaxLength(100);
                b.Property(u => u.PasswordHash).IsRequired();
            });

            builder.Entity<Article>(b =>
            {
                b.HasKey(a => a.Id);
                b.Property(a => a.Title).IsRequired().HasMaxLength(200);
                b.Property(a => a.Description).IsRequired().HasMaxLength(500);
                b.Property(a => a.Content).IsRequired();
                b.HasOne(a => a.Author).WithMany(u => u.Articles).HasForeignKey(a => a.AuthorId).OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<Tag>(b =>
            {
                b.HasKey(t => t.Id);
                b.Property(t => t.Name).IsRequired().HasMaxLength(50);
            });

            builder.Entity<ArticleTag>(b =>
            {
                b.HasKey(at => new { at.ArticleId, at.TagId });
                b.HasOne(at => at.Article).WithMany(a => a.ArticleTags).HasForeignKey(at => at.ArticleId);
                b.HasOne(at => at.Tag).WithMany(t => t.ArticleTags).HasForeignKey(at => at.TagId);
            });
        }
    }
}
