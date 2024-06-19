using Backend.Application.Common.Paging;
using Backend.Domain.Entities;
using Backend.Domain.Enum;

namespace Backend.Application.IRepositories
{
    public interface IAssetRepository : IBaseRepository<Asset>
    {
        Task<Asset> GenerateAssetInfo(Asset asset);
        Task<PaginationResponse<Asset>> GetFilterAsync(AssetFilterRequest request, Location location);
    }
}