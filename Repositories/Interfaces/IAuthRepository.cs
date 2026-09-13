using Personal_Blogging_Platform_API.Models;

namespace Personal_Blogging_Platform_API.Repositories.Interfaces
{
    public interface IAuthRepository
    {
        Task<User> GetByEmailAsync(string email);
        Task<User> GetByIdAsync(int id);
        Task AddAsync(User user);
        Task SaveChangesAsync();
    }
}
