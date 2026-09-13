using Personal_Blogging_Platform_API.DTOs.Tags;

namespace Personal_Blogging_Platform_API.Services.Interfaces
{
    public interface ITagService
    {
        Task<IEnumerable<string>> GetAllAsync();
        Task<TagDto> CreateAsync(TagDto dto);
    }
}
