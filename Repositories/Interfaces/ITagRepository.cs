using Personal_Blogging_Platform_API.Models;

namespace Personal_Blogging_Platform_API.Repositories.Interfaces
{
    public interface ITagRepository
    {
        Task<Tag> GetByNameAsync(string name);
        Task<IEnumerable<Tag>> GetAllAsync();
        Task AddAsync(Tag tag);
        Task SaveChangesAsync();
    }
}
