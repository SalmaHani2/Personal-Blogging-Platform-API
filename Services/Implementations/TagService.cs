using Personal_Blogging_Platform_API.DTOs.Tags;
using Personal_Blogging_Platform_API.Models;
using Personal_Blogging_Platform_API.Repositories.Interfaces;
using Personal_Blogging_Platform_API.Services.Interfaces;

namespace Personal_Blogging_Platform_API.Services.Implementations
{
    public class TagService : ITagService
    {
        private readonly ITagRepository _repo;
        public TagService(ITagRepository repo) { _repo = repo; }

        public async Task<TagDto> CreateAsync(TagDto dto)
        {
            var existing = await _repo.GetByNameAsync(dto.Name);
            if (existing != null) throw new ApplicationException("Tag already exists");

            var tag = new Tag { Name = dto.Name };
            await _repo.AddAsync(tag);
            await _repo.SaveChangesAsync();

            return dto;
        }

        public async Task<IEnumerable<string>> GetAllAsync()
        {
            var tags = await _repo.GetAllAsync();
            return tags.Select(t => t.Name);
        }
    }
}
