using Backend.Domain.Entity;

namespace Backend.Application.IRepositories
{
    public interface ICategoryRepository : IBaseRepository<Category>
    {
        Task<Category?> FindCategoryByNameAsync(string name);
        Task<Category?> FindCategoryByPrefixAsync(string prefix);
        Task<IEnumerable<Category>> GetFilterAsync(string? searchTerm);
        Task<IEnumerable<Category>> GetAllCategoriesAsync();
    }
}