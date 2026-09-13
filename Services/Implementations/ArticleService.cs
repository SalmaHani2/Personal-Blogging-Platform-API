using Personal_Blogging_Platform_API.DTOs.Articles;
using Personal_Blogging_Platform_API.Models;
using Personal_Blogging_Platform_API.Repositories.Interfaces;
using Personal_Blogging_Platform_API.Services.Interfaces;

namespace Personal_Blogging_Platform_API.Services.Implementations
{
    public class ArticleService : IArticleService
    {
        private readonly IArticleRepository _repo;
        private readonly ITagRepository _tagRepo;

        public ArticleService(IArticleRepository repo, ITagRepository tagRepo)
        {
            _repo = repo;
            _tagRepo = tagRepo;
        }

        public async Task<ArticleResponseDto> CreateAsync(ArticleCreateDto dto, int userId)
        {
            var article = new Article
            {
                Title = dto.Title,
                Description = dto.Description,
                Content = dto.Content,
                AuthorId = userId,
                PublishedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow
            };

            // handle tags
            foreach (var tagName in dto.Tags.Distinct())
            {
                var tag = await _tagRepo.GetByNameAsync(tagName);
                if (tag == null)
                {
                    tag = new Tag { Name = tagName };
                    await _tagRepo.AddAsync(tag);
                    await _tagRepo.SaveChangesAsync();
                }

                article.ArticleTags.Add(new ArticleTag { TagId = tag.Id, Article = article });
            }

            await _repo.AddAsync(article);
            await _repo.SaveChangesAsync();

            return await GetByIdAsync(article.Id);
        }

        public async Task<bool> DeleteAsync(int id, int userId, string userRole)
        {
            var article = await _repo.GetByIdAsync(id);
            if (article == null) return false;
            if (article.AuthorId != userId && userRole != "Admin") throw new UnauthorizedAccessException();

            _repo.Remove(article);
            await _repo.SaveChangesAsync();
            return true;
        }

        public async Task<(IEnumerable<ArticleResponseDto> Items, int TotalCount, int CurrentPage, int PageSize)> GetAllAsync(string tag, string title, string author, DateTime? publishedDate, int pageNumber, int pageSize)
        {
            var (items, total) = await _repo.GetFilteredAsync(tag, title, author, publishedDate, pageNumber <= 0 ? 1 : pageNumber, pageSize <= 0 ? 10 : pageSize);

            var dtoItems = items.Select(a => new ArticleResponseDto
            {
                Id = a.Id,
                Title = a.Title,
                Description = a.Description,
                Content = a.Content,
                PublishedDate = a.PublishedDate,
                UpdatedDate = a.UpdatedDate,
                AuthorId = a.AuthorId,
                AuthorUsername = a.Author?.Username,
                Tags = a.ArticleTags.Select(at => at.Tag.Name).ToList()
            });

            return (dtoItems, total, pageNumber, pageSize);
        }

        public async Task<ArticleResponseDto> GetByIdAsync(int id)
        {
            var a = await _repo.GetByIdAsync(id);
            if (a == null) return null;
            return new ArticleResponseDto
            {
                Id = a.Id,
                Title = a.Title,
                Description = a.Description,
                Content = a.Content,
                PublishedDate = a.PublishedDate,
                UpdatedDate = a.UpdatedDate,
                AuthorId = a.AuthorId,
                AuthorUsername = a.Author?.Username,
                Tags = a.ArticleTags.Select(at => at.Tag.Name).ToList()
            };
        }

        public async Task<bool> UpdateAsync(int id, ArticleUpdateDto dto, int userId, string userRole)
        {
            var article = await _repo.GetByIdAsync(id);
            if (article == null) return false;
            if (article.AuthorId != userId && userRole != "Admin") throw new UnauthorizedAccessException();

            article.Title = dto.Title;
            article.Description = dto.Description;
            article.Content = dto.Content;
            article.UpdatedDate = DateTime.UtcNow;

            // update tags: simple approach remove existing and add new
            article.ArticleTags.Clear();
            foreach (var tagName in dto.Tags.Distinct())
            {
                var tag = await _tagRepo.GetByNameAsync(tagName);
                if (tag == null)
                {
                    tag = new Tag { Name = tagName };
                    await _tagRepo.AddAsync(tag);
                    await _tagRepo.SaveChangesAsync();
                }
                article.ArticleTags.Add(new ArticleTag { TagId = tag.Id, ArticleId = article.Id });
            }

            await _repo.SaveChangesAsync();
            return true;
        }
    }
}
