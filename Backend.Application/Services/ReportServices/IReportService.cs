using Backend.Application.Common.Paging;
using Backend.Domain.Entities;

namespace Backend.Application.Services.ReportServices
{
    public interface IReportService
    {
        Task<byte[]> ExportAssetReportAsync(string? SortColumn, string? SortOrder);
        Task<PaginationResponse<AssetReport>> GetAssetReportAsync(string? SortColumn, string? SortDirection, int? PageSize, int? Page);
    }
}