using Backend.Application.DTOs.AssetDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.IRepositories
{
    public interface IReportRepository
    {
        Task<List<AssetReportDto>> GetAssetReportAsync(string SortColumn, string SortDirection, int PageSize, int Page);

    }
}
