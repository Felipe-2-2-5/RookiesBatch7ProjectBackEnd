using Backend.Domain.Entities;

namespace Backend.Application.IRepositories
{
    public interface IAssetRepository : IBaseRepository<Asset>
    {
        Task<Asset> GenerateAssetInfo(Asset asset);
    }
}