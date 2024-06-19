using Backend.Application.Common.Paging;
using Backend.Application.DTOs.AssetDTOs;
using Backend.Domain.Entities;
using Backend.Domain.Enum;

namespace Backend.Application.Services.AssetServices
{
    public interface IAssetService
    {
        Task<AssetResponseDTO> InsertAsync(AssetDTO assetDTO, string createName, Location location);
        Task<AssetResponseDTO> GetByIdAsync(int id);
        Task<PaginationResponse<AssetResponseDTO>> GetFilterAsync(AssetFilterRequest request, Location location);
    }
}