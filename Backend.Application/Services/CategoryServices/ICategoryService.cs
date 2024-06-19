using Backend.Application.DTOs.CategoryDTOs;

namespace Backend.Application.Services.CategoryServices
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryResponseDTO>> GetFilterAsync(string? searchTerm);
        Task<CategoryResponseDTO> InsertAsync(CategoryDTO dto);
    }
}