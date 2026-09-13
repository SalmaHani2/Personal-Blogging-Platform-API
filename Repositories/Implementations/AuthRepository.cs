using Microsoft.EntityFrameworkCore;
using Personal_Blogging_Platform_API.Data;
using Personal_Blogging_Platform_API.Models;
using Personal_Blogging_Platform_API.Repositories.Interfaces;

namespace Personal_Blogging_Platform_API.Repositories.Implementations
{
    public class AuthRepository : IAuthRepository
    {
        private readonly ApplicationDbContext _db;
        public AuthRepository(ApplicationDbContext db) { _db = db; }

        public async Task AddAsync(User user)
        {
            await _db.Users.AddAsync(user);
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await _db.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User> GetByIdAsync(int id)
        {
            return await _db.Users.FindAsync(id);
        }

        public async Task SaveChangesAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
