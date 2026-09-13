using Microsoft.EntityFrameworkCore;
using Personal_Blogging_Platform_API.Data;
using Personal_Blogging_Platform_API.Models;
using Personal_Blogging_Platform_API.Repositories.Interfaces;

namespace Personal_Blogging_Platform_API.Repositories.Implementations
{
    public class ArticleRepository : IArticleRepository
    {
        private readonly ApplicationDbContext _db;
        public ArticleRepository(ApplicationDbContext db) { _db = db; }

        public async Task AddAsync(Article article)
        {
            await _db.Articles.AddAsync(article);
        }

        public async Task<IEnumerable<Article>> GetAllAsync()
        {
            return await _db.Articles.Include(a => a.Author).Include(a => a.ArticleTags).ThenInclude(at => at.Tag).ToListAsync();
        }

        public async Task<Article> GetByIdAsync(int id)
        {
            return await _db.Articles.Include(a => a.Author).Include(a => a.ArticleTags).ThenInclude(at => at.Tag).FirstOrDefaultAsync(a => a.Id == id);
        }

        public void Remove(Article article)
        {
            _db.Articles.Remove(article);
        }

        public async Task SaveChangesAsync()
        {
            await _db.SaveChangesAsync();
        }

        public async Task<(IEnumerable<Article> Items, int TotalCount)> GetFilteredAsync(string tag, string title, string author, DateTime? publishedDate, int pageNumber, int pageSize)
        {
            var query = _db.Articles.Include(a => a.Author).Include(a => a.ArticleTags).ThenInclude(at => at.Tag).AsQueryable();

            if (!string.IsNullOrWhiteSpace(tag))
            {
                query = query.Where(a => a.ArticleTags.Any(at => at.Tag.Name == tag));
            }

            if (!string.IsNullOrWhiteSpace(title))
            {
                query = query.Where(a => a.Title.Contains(title));
            }

            if (!string.IsNullOrWhiteSpace(author))
            {
                query = query.Where(a => a.Author.Username.Contains(author));
            }

            if (publishedDate.HasValue)
            {
                var d = publishedDate.Value.Date;
                query = query.Where(a => a.PublishedDate.Date == d);
            }

            var total = await query.CountAsync();

            var items = await query.OrderByDescending(a => a.PublishedDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, total);
        }
    }
}
