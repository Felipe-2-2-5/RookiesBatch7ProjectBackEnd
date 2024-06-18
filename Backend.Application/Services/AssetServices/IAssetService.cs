using Backend.Application.DTOs.AssetDTOs;
using Backend.Domain.Enum;

namespace Backend.Application.Services.AssetServices
{
    public interface IAssetService
    {
        Task<AssetResponseDTO> GetByIdAsync(int id);
        Task InsertAsync(AssetDTO assetDTO, string createName, Location location);
    }
}