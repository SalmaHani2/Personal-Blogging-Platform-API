using Personal_Blogging_Platform_API.Models;

namespace Personal_Blogging_Platform_API.Repositories.Interfaces
{
    public interface IArticleRepository
    {
        Task<Article> GetByIdAsync(int id);
        Task<IEnumerable<Article>> GetAllAsync();
        Task AddAsync(Article article);
        void Remove(Article article);
        Task SaveChangesAsync();
        Task<(IEnumerable<Article> Items, int TotalCount)> GetFilteredAsync(string tag, string title, string author, DateTime? publishedDate, int pageNumber, int pageSize);
    }
}
