using Backend.Application.Common.Paging;
using Backend.Application.IRepositories;
using Backend.Domain.Entities;
using Backend.Domain.Enum;
using Backend.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Backend.Infrastructure.Repositories
{
    public class AssignmentRepository : BaseRepository<Assignment>, IAssignmentRepository
    {
        public AssignmentRepository(AssetContext context) : base(context)
        {
        }
        public override async Task<Assignment?> GetByIdAsync(int id)
        {
            return await _context.Assignments
                .Include(a => a.AssignedTo)
                .Include(a => a.AssignedBy)
                .Include(a => a.Asset)
                .ThenInclude(a => a!.Category)
                .FirstOrDefaultAsync(a => a.Id == id);
        }
        public async Task<PaginationResponse<Assignment>> GetFilterAsync(AssignmentFilterRequest request, Location location)
        {
            IQueryable<Assignment> query = _table.Where(a => a.IsDeleted == false && a.AssignedDate <= DateTime.Now && a.Asset.Location == location)
                .Include(a => a.Asset)
                .Include(a => a.AssignedTo)
            //IQueryable<Assignment> query = _table.Where(a => a.IsDeleted == false && a.AssignedDate <= DateTime.Now && a.Asset.Location == location)

                .Include(a => a.Asset)
                .Include(a => a.AssignedTo)
                .Include(a => a.AssignedBy);

            if (!string.IsNullOrWhiteSpace(request.State))
            {
                query = request.State == "Accepted" ? query.Where(p => p.State == AssignmentState.Accepted) : query.Where(p => p.State == AssignmentState.Waiting);
            }
            if (request.FromDate.HasValue && request.FromDate.Value != DateTime.MinValue)
            {
                query = query.Where(p => p.AssignedDate >= request.FromDate.Value);
            }

            if (request.ToDate.HasValue && request.ToDate.Value != DateTime.MinValue)
            {
                query = query.Where(p => p.AssignedDate <= request.ToDate.Value);
            }
            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                query = query.Where(p =>
                    p.Asset!.AssetCode.Contains(request.SearchTerm) ||
                    p.Asset.AssetName.Contains(request.SearchTerm) ||
                    p.AssignedTo!.UserName.Contains(request.SearchTerm));
            }

            query = request.SortOrder?.ToLower() == "descend" ? query.OrderByDescending(GetSortProperty(request)) : query.OrderBy(GetSortProperty(request));
            var totalCount = await query.CountAsync();
            var items = await query.Skip((request.Page - 1) * request.PageSize).Take(request.PageSize).AsNoTracking().ToListAsync();
            return new PaginationResponse<Assignment>(items, totalCount);
        }

        private static Expression<Func<Assignment, object>> GetSortProperty(AssignmentFilterRequest request) =>
            request.SortColumn?.ToLower() switch
            {
                "code" => asset => asset.Asset!.AssetCode,
                "name" => asset => asset.Asset!.AssetName,
                "receiver" => asset => asset.AssignedTo!.UserName,
                "provider" => asset => asset.AssignedBy!.UserName,
                "date" => asset => asset.AssignedDate,
                "state" => asset => asset.State,
                _ => asset => new { asset.AssignedDate, asset.Asset!.AssetCode }
            };

        public async Task<Assignment?> FindAssignmentByAssetIdAsync(int assetId)
        {
            return await _context.Assignments
                            .Include(a => a.Asset)
                            .Include(a => a.AssignedTo)
                            .Include(a => a.AssignedBy)
                            .AsNoTracking().FirstOrDefaultAsync(a => a.AssetId == assetId);
        }

        public async Task<Assignment?> FindAssignmentByIdWithoutAsset(int id)
        {
            return await _context.Assignments
                            .Include(a => a.AssignedTo)
                            .Include(a => a.AssignedBy)
                            .FirstOrDefaultAsync(a => a.Id == id);
        }
        public async Task<Assignment?> FindLatestAssignment()
        {
            return await _context.Assignments
                            .Include(a => a.Asset)
                            .Include(a => a.AssignedTo)
                            .Include(a => a.AssignedBy)
                            .OrderByDescending(a => a.Id)
                            .AsNoTracking().FirstOrDefaultAsync();
        }
        public async Task<PaginationResponse<Assignment>> GetMyAssignmentsAsync(MyAssignmentFilterRequest request)
        {
            IQueryable<Assignment> query = _table.Where(a => a.IsDeleted == false && a.AssignedDate <= DateTime.Now)
                .Include(a => a.Asset)
                .Include(a => a.AssignedTo)
                .Include(a => a.AssignedBy);

            query = request.SortOrder?.ToLower() == "descend" ? query.OrderByDescending(GetSortPropertyMyAssignment(request)) : query.OrderBy(GetSortPropertyMyAssignment(request));
            var totalCount = await query.CountAsync();
            var items = await query.Skip((request.Page - 1) * request.PageSize).Take(request.PageSize).AsNoTracking().ToListAsync();
            return new PaginationResponse<Assignment>(items, totalCount);
        }
        private static Expression<Func<Assignment, object>> GetSortPropertyMyAssignment(MyAssignmentFilterRequest request) =>
            request.SortColumn?.ToLower() switch
            {
                "code" => asset => asset.Asset!.AssetCode,
                "name" => asset => asset.Asset!.AssetName,
                "date" => asset => asset.AssignedDate,
                "state" => asset => asset.State,
                _ => asset => new { asset.AssignedDate, asset.Asset!.AssetCode }
            };
    }
}