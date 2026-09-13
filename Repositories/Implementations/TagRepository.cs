using Microsoft.EntityFrameworkCore;
using Personal_Blogging_Platform_API.Data;
using Personal_Blogging_Platform_API.Models;
using Personal_Blogging_Platform_API.Repositories.Interfaces;

namespace Personal_Blogging_Platform_API.Repositories.Implementations
{
    public class TagRepository : ITagRepository
    {
        private readonly ApplicationDbContext _db;
        public TagRepository(ApplicationDbContext db) { _db = db; }

        public async Task AddAsync(Tag tag)
        {
            await _db.Tags.AddAsync(tag);
        }

        public async Task<IEnumerable<Tag>> GetAllAsync()
        {
            return await _db.Tags.ToListAsync();
        }

        public async Task<Tag> GetByNameAsync(string name)
        {
            return await _db.Tags.FirstOrDefaultAsync(t => t.Name == name);
        }

        public async Task SaveChangesAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
