using Backend.Application.DTOs.CategoryDTOs;

namespace Backend.Application.Services.CategoryServices
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryResponse>> GetFilterAsync(string? searchTerm);
        Task<CategoryResponse> InsertAsync(CategoryDTO dto);
        Task<IEnumerable<CategoryResponse>> GetAllAsync();
    }
}