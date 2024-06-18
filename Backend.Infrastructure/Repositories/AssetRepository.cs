using Backend.Application.IRepositories;
using Backend.Domain.Entities;
using Backend.Infrastructure.Data;
using Backend.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure.Repositories
{
    public class AssetRepository : BaseRepository<Asset>, IAssetRepository
    {
        public AssetRepository(AssetContext dbContext) : base(dbContext)
        {
        }
        public async Task<Asset> GenerateAssetInfo(Asset asset)
        {
            var category = await _context.Categories.FindAsync(asset.CategoryId);
            var users = await _table
                .Where(s => s.AssetCode.StartsWith(category.Prefix))
                .Select(s => s.AssetCode).ToListAsync();

            var numbers = users
                .Select(u =>
                {
                    string suffix = u.Substring(category.Prefix.Length);
                    return int.TryParse(suffix, out int n) ? n : 0;
                })
                .ToList();

            int maxNumber = numbers.Count > 0 ? numbers.Max() + 1 : 1; // Start from 1 if no existing codes

            string maxNumberString = maxNumber.ToString("D6");
            var assetCode = $"{category.Prefix}{maxNumberString.PadLeft(6, '0')}"; // Format as [Prefix][000001]
            asset.AssetCode = assetCode;
            return asset;
        }
        public async Task<Asset?> FindAssetByCodeAsync(string code) => await _table.AsNoTracking().FirstOrDefaultAsync(u => u.AssetCode == code);

    }

}
