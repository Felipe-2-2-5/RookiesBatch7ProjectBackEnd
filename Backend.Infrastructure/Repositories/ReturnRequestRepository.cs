using Backend.Application.Common.Paging;
using Backend.Application.IRepositories;
using Backend.Domain.Entities;
using Backend.Domain.Enum;
using Backend.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Backend.Infrastructure.Repositories
{
    public class ReturnRequestRepository : BaseRepository<ReturnRequest>, IReturnRequestRepository
    {
        public ReturnRequestRepository(AssetContext dbContext) : base(dbContext)
        {
        }

        public override async Task<ReturnRequest?> GetByIdAsync(int id)
        {
            return await _context.ReturnRequests
                 .Include(a => a.Assignment)
                    .ThenInclude(assignment => assignment!.Asset)
                .Include(a => a.Requestor)
                .Include(a => a.Acceptor)
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<PaginationResponse<ReturnRequest>> GetFilterAsync(ReturnRequestFilterRequest request, Location location)
        {
            IQueryable<ReturnRequest> query = _table.Where(a => a.Assignment!.Asset!.Location == location)
                .Include(a => a.Assignment)
                    .ThenInclude(assignment => assignment!.Asset)
                .Include(a => a.Requestor)
                .Include(a => a.Acceptor);

            if (!string.IsNullOrWhiteSpace(request.State))
            {
                query = request.State == ReturnRequestState.Completed.ToString() ? query.Where(p => p.State == ReturnRequestState.Completed) : query.Where(p => p.State == ReturnRequestState.WaitingForReturning);
            }

            if (request.ReturnedDate.HasValue && request.ReturnedDate.Value != DateTime.MinValue)
            {
                query = query.Where(p =>
                    p.ReturnedDate.HasValue &&
                    p.ReturnedDate.Value.Date == request.ReturnedDate.Value.Date);
            }

            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                query = query.Where(p => p.Assignment!.Asset!.AssetCode.Contains(request.SearchTerm) || p.Assignment!.Asset.AssetName.Contains(request.SearchTerm) || p.Requestor!.UserName.Contains(request.SearchTerm));
            }

            query = request.SortOrder?.ToLower() == "descend"
                ? query.OrderByDescending(GetSortProperty(request))
                : query.OrderBy(GetSortProperty(request));

            var totalCount = await query.CountAsync();
            var items = await query.Skip((request.Page - 1) * request.PageSize).Take(request.PageSize).AsNoTracking().ToListAsync();
            return new(items, totalCount);
        }

        private static Expression<Func<ReturnRequest, object>> GetSortProperty(ReturnRequestFilterRequest request) =>
            request.SortColumn switch
            {
                "assetCode" => returnRequest => returnRequest.Assignment!.Asset!.AssetCode,
                "assetName" => returnRequest => returnRequest.Assignment!.Asset!.AssetName,
                "requestedBy" => returnRequest => returnRequest.Requestor!.UserName!,
                "assignedDate" => returnRequest => returnRequest.Assignment!.AssignedDate!,
                "acceptedBy" => returnRequest => returnRequest.Acceptor!.UserName!,
                "returnedDate" => returnRequest => returnRequest.ReturnedDate!,
                "state" => returnRequest => returnRequest.State,
                _ => returnRequest => returnRequest.ReturnedDate!
            };
    }
}
