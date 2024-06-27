using Backend.Application.DTOs.AssetDTOs;
using Backend.Application.IRepositories;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class ReportRepository : IReportRepository
{
    private readonly DbContext _context;

    public ReportRepository(DbContext context)
    {
        _context = context;
    }

    public async Task<List<AssetReportDto>> GetAssetReportAsync(string sortColumn, string sortDirection)
    {
        var sortColumnParameter = new SqlParameter("@SortColumn", sortColumn ?? "Category");
        var sortDirectionParameter = new SqlParameter("@SortDirection", sortDirection ?? "ASC");

        return await _context.Set<AssetReportDto>()
            .FromSqlRaw("EXEC GetAssetReport @SortColumn, @SortDirection", sortColumnParameter, sortDirectionParameter)
            .ToListAsync();
    }
}
