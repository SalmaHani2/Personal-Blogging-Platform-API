using Personal_Blogging_Platform_API.DTOs.Articles;

namespace Personal_Blogging_Platform_API.Services.Interfaces
{
    public interface IArticleService
    {
        Task<(IEnumerable<ArticleResponseDto> Items, int TotalCount, int CurrentPage, int PageSize)> GetAllAsync(string tag, string title, string author, DateTime? publishedDate, int pageNumber, int pageSize);
        Task<ArticleResponseDto> GetByIdAsync(int id);
        Task<ArticleResponseDto> CreateAsync(ArticleCreateDto dto, int userId);
        Task<bool> UpdateAsync(int id, ArticleUpdateDto dto, int userId, string userRole);
        Task<bool> DeleteAsync(int id, int userId, string userRole);
    }
}
