using System.Linq.Expressions;
using Backend.Application.Common.Paging;
using Backend.Application.IRepositories;
using Backend.Domain.Entities;
using Backend.Domain.Enum;
using Backend.Infrastructure.Data;
using Backend.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure.Repositories;

public class AssignmentRepository : BaseRepository<Assignment>, IAssignmentRepository
{
    public AssignmentRepository(AssetContext context) : base(context)
    {
    }
    public async Task<PaginationResponse<Assignment>> GetFilterAsync(AssignmentFilterRequest request)
    {
        IQueryable<Assignment> query = _table.Where(a => a.IsDeleted == true);

        if (!string.IsNullOrWhiteSpace(request.State))
        {
            query = request.State == "Accepted" ? query.Where(p => p.State == AssignmentState.Accepted) : query.Where(p => p.State == AssignmentState.Waiting);
        }
        if (request.FromDate != DateTime.MinValue)
        {
            query = query.Where(p => p.AssignedDate >= request.FromDate);
        }
        if (request.ToDate != DateTime.MinValue)
        {
            query = query.Where(p => p.AssignedDate <= request.ToDate);
        }
        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            query = query.Where(p =>
                p.Asset.AssetCode.Contains(request.SearchTerm) ||
                p.Asset.AssetName.Contains(request.SearchTerm) ||
                p.AssignedTo.UserName.Contains(request.SearchTerm));
        }

        query = request.SortOrder?.ToLower() == "descend" ? query.OrderByDescending(GetSortProperty(request)) : query.OrderBy(GetSortProperty(request));
        var totalCount = await query.CountAsync();
        var items = await query.Skip((request.Page - 1) * request.PageSize).Take(request.PageSize).AsNoTracking().ToListAsync();
        return new PaginationResponse<Assignment>(items, totalCount);
    }
    
    private static Expression<Func<Assignment, object>> GetSortProperty(AssignmentFilterRequest request) =>
        request.SortColumn?.ToLower() switch
        {
            "assetCode" => asset => asset.Asset.AssetCode,
            "assetName" => asset => asset.Asset.AssetName,
            "assignedTo" => asset => asset.AssignedTo.UserName,
            "assignedBy" => asset => asset.AssignedBy.UserName,
            "dateAssigned" => asset => asset.AssignedDate,
            "state" => asset => asset.State,
            _ => asset => asset.AssignedDate
        };
}