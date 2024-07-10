using Backend.Application.Common.Paging;
using Backend.Domain.Entities;

namespace Backend.Application.IRepositories
{
    public interface IReportRepository
    {
        Task<PaginationResponse<AssetReport>> GetAssetReportAsync(string? SortColumn, string? SortDirection, int? PageSize, int? Page);
    }
}
