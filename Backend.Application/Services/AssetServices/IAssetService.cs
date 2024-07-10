using Backend.Application.Common.Paging;
using Backend.Application.DTOs.AssetDTOs;
using Backend.Application.DTOs.AuthDTOs;
using Backend.Domain.Enum;

namespace Backend.Application.Services.AssetServices
{
    public interface IAssetService
    {
        Task<AssetResponse> InsertAsync(AssetDTO assetDTO, string createName, Location location);
        Task<AssetResponse> GetByIdAsync(int id);
        Task<PaginationResponse<AssetResponse>> GetFilterAsync(AssetFilterRequest request, Location location);
        Task<PaginationResponse<AssetResponse>> GetFilterChoosingAsync(int id, AssetFilterRequest request, Location location);
        Task<AssetResponse> UpdateAsync(int id, AssetDTO assetDTO, string updatedName);
        Task DeleteAsync(int id);
    }
}