using Backend.Application.DTOs.AssetDTOs;
using Backend.Domain.Enum;

namespace Backend.Application.Services.AssetServices
{
    public interface IAssetService
    {
        Task<AssetResponseDTO> GetByIdAsync(int id);
        Task<AssetResponseDTO> InsertAsync(AssetDTO assetDTO, string createName, Location location);
    }
}