using Backend.Application.Common.Paging;
using Backend.Application.IRepositories;
using Backend.Domain.Entities;
using Backend.Domain.Enum;
using Backend.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

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
                .Where(s => s.AssetCode.StartsWith(category!.Prefix))
                .Select(s => s.AssetCode).ToListAsync();

            var numbers = users
                .Select(u =>
                {
                    string suffix = u.Substring(category!.Prefix.Length);
                    return int.TryParse(suffix, out int n) ? n : 0;
                })
                .ToList();

            int maxNumber = numbers.Count > 0 ? numbers.Max() + 1 : 1; // Start from 1 if no existing codes

            string maxNumberString = maxNumber.ToString("D6");
            var assetCode = $"{category!.Prefix}{maxNumberString.PadLeft(6, '0')}"; // Format as [Prefix][000001]
            asset.AssetCode = assetCode;
            return asset;
        }
        public async Task<Asset?> FindAssetByCodeAsync(string code) =>
            await _table.Include(a => a.Category)
                .Include(a => a.Assignments).AsNoTracking().FirstOrDefaultAsync(u => u.AssetCode == code);

        public override async Task<Asset?> GetByIdAsync(int id)
        {
            return await _context.Assets
                .Include(a => a.Category)
                .Include(a => a.Assignments!)
                    .ThenInclude(assignment => assignment.AssignedBy)
                .Include(a => a.Assignments!)
                    .ThenInclude(assignment => assignment.AssignedTo)
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<PaginationResponse<Asset>> GetFilterAsync(AssetFilterRequest request, Location location)
        {
            IQueryable<Asset> query = _table.Where(u => u.Location == location)
                .Include(u => u.Category)
                .Include(u => u.Assignments);

            query = request.State switch
            {
                "All" => query.Where(p =>
                    p.State == AssetState.Available ||
                    p.State == AssetState.NotAvailable ||
                    p.State == AssetState.WaitingForRecycling ||
                    p.State == AssetState.Assigned ||
                    p.State == AssetState.Recycled),
                "Available" => query.Where(p => p.State == AssetState.Available),
                "Not available" => query.Where(p => p.State == AssetState.NotAvailable),
                "Waiting for Recycling" => query.Where(p => p.State == AssetState.WaitingForRecycling),
                "Recycled" => query.Where(p => p.State == AssetState.Recycled),
                "Assigned" => query.Where(p => p.State == AssetState.Assigned),
                _ => query.Where(p => p.State == AssetState.Available || p.State == AssetState.NotAvailable || p.State == AssetState.Assigned)
            };

            if (!string.IsNullOrWhiteSpace(request.Category))
            {
                query = query.Where(p => p.Category!.Name == request.Category);
            }

            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                query = query.Where(p => p.AssetCode.Contains(request.SearchTerm) || p.AssetName.Contains(request.SearchTerm));
            }

            query = request.SortOrder?.ToLower() == "descend"
                ? query.OrderByDescending(GetSortProperty(request))
                : query.OrderBy(GetSortProperty(request));

            var totalCount = await query.CountAsync();
            var items = await query.Skip((request.Page - 1) * request.PageSize).Take(request.PageSize).AsNoTracking().ToListAsync();
            return new(items, totalCount);
        }

        private static Expression<Func<Asset, object>> GetSortProperty(AssetFilterRequest request) =>
            request.SortColumn?.ToLower() switch
            {
                "assetcode" => asset => asset.AssetCode,
                "assetname" => asset => asset.AssetName,
                "category" => asset => asset.Category!.Name,
                "state" => asset => asset.State,
                _ => user => user.AssetName
            };
    }

}
