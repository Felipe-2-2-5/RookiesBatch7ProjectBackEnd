using Backend.Domain.Entities;

namespace Backend.Application.IRepositories
{
    public interface IAssetRepository : IBaseRepository<Asset>
    {
        Task<Asset?> FindAssetByCodeAsync(string code);
        Task<Asset> GenerateAssetInfo(Asset asset);
    }
}