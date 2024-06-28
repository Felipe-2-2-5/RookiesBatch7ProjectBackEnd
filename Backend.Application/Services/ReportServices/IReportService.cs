using Backend.Application.Common.Paging;
using Backend.Application.DTOs.AssetDTOs;
using Backend.Domain.Enum;

namespace Backend.Application.Services.ReportServices
{
    public interface IReportService
    {
        Task<List<AssetReportDto>> GetAssetReportAsync(string SortColumn, string SortDirection, int PageSize, int Page);
    }
}