using Backend.Application.IRepositories;
using Backend.Domain.Entity;
using Backend.Infrastructure.Data;
using Backend.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure.Repositories
{
    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(AssetContext dbContext) : base(dbContext)
        {
        }
        public async Task<IEnumerable<Category>> GetFilterAsync(string? searchTerm)
        {
            IQueryable<Category> query = _table;
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(p =>
                p.Prefix.Contains(searchTerm) ||
                    p.Name.Contains(searchTerm));
            }
            return await query.ToListAsync();
        }
        public async Task<Category?> FindCategoryByNameAsync(string name) => await _table.AsNoTracking().FirstOrDefaultAsync(u => u.Name == name);
        public async Task<Category?> FindCategoryByPrefixAsync(string prefix) => await _table.AsNoTracking().FirstOrDefaultAsync(u => u.Prefix == prefix);

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            return await _table.ToListAsync();
        }
    }
}
