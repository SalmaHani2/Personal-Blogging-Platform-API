using Microsoft.EntityFrameworkCore;
using Personal_Blogging_Platform_API.Models;
using Personal_Blogging_Platform_API.Services.Implementations;

namespace Personal_Blogging_Platform_API.Data
{
    public static class DbInitializer
    {
        public static async Task SeedAsync(ApplicationDbContext context)
        {
            if (await context.Users.AnyAsync()) return; // already seeded

            var authService = new AuthService(null, null); // use static helper for hashing

            var admin = new User
            {
                Username = "admin",
                Email = "admin@example.com",
                Role = "Admin",
                CreatedAt = DateTime.UtcNow,
                PasswordHash = AuthService.HashPasswordStatic("Admin123!")
            };

            var user = new User
            {
                Username = "salma",
                Email = "salma@example.com",
                Role = "User",
                CreatedAt = DateTime.UtcNow,
                PasswordHash = AuthService.HashPasswordStatic("Password123!")
            };

            await context.Users.AddRangeAsync(admin, user);

            var tag1 = new Tag { Name = "csharp" };
            var tag2 = new Tag { Name = "aspnet" };
            var tag3 = new Tag { Name = "tutorial" };

            await context.Tags.AddRangeAsync(tag1, tag2, tag3);

            var article1 = new Article
            {
                Title = "Getting started with ASP.NET Core",
                Description = "A short guide to start web APIs.",
                Content = "Content for ASP.NET Core article...",
                PublishedDate = DateTime.UtcNow.AddDays(-10),
                UpdatedDate = DateTime.UtcNow.AddDays(-9),
                Author = user
            };

            var article2 = new Article
            {
                Title = "C# Tips",
                Description = "Useful tips for C# developers.",
                Content = "Content for C# tips...",
                PublishedDate = DateTime.UtcNow.AddDays(-5),
                UpdatedDate = DateTime.UtcNow.AddDays(-4),
                Author = admin
            };

            await context.Articles.AddRangeAsync(article1, article2);

            await context.SaveChangesAsync();

            // Link tags
            context.ArticleTags.Add(new ArticleTag { ArticleId = article1.Id, TagId = tag2.Id });
            context.ArticleTags.Add(new ArticleTag { ArticleId = article1.Id, TagId = tag3.Id });
            context.ArticleTags.Add(new ArticleTag { ArticleId = article2.Id, TagId = tag1.Id });

            await context.SaveChangesAsync();
        }
    }
}
