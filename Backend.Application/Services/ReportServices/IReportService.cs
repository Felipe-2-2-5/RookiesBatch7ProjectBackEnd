using Backend.Application.Common.Paging;
using Backend.Application.DTOs.AssetDTOs;

namespace Backend.Application.Services.ReportServices
{
    public interface IReportService
    {
        Task<byte[]> ExportAssetReportAsync(string? SortColumn, string? SortOrder);
        Task<PaginationResponse<AssetReportDto>> GetAssetReportAsync(string? SortColumn, string? SortDirection, int? PageSize, int? Page);
    }
}