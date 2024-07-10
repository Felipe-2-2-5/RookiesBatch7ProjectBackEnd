using Backend.Application.Common.Paging;

using Backend.Application.IRepositories;

using Backend.Domain.Entities;

using Backend.Infrastructure.Data;

using Microsoft.Data.SqlClient;

using Microsoft.EntityFrameworkCore;



public class ReportRepository : IReportRepository
{
    private readonly AssetContext _context;
    private readonly string _connectionString;

    public ReportRepository(AssetContext context)
    {
        _context = context;
        _connectionString = context.Database.GetConnectionString()!;
    }

    public async Task<PaginationResponse<AssetReport>> GetAssetReportAsync(string? sortColumn, string? sortDirection, int? pageSize = 20, int? page = 1)
    {
        var sortColumnParameter = new SqlParameter("@SortColumn", sortColumn ?? "Category");

        var sortDirectionParameter = new SqlParameter("@SortDirection", sortDirection ?? "ASC");

        // Build the SQL query
        string sqlQuery = $@"
            EXEC [dbo].[getAssetReportByCategoryAndState] 
                @SortColumn = @SortColumn, 
                @SortDirection = @SortDirection";

        // Execute the stored procedure and get the results
        var results = await _context.AssetsReport.FromSqlRaw(sqlQuery, sortColumnParameter, sortDirectionParameter).ToListAsync();

        // Paginate the results
        var paginatedResults = results.Skip((int)page! - 1 * pageSize!.Value).Take(pageSize.Value).ToList();

        // Create and return the response
        var response = new PaginationResponse<Backend.Domain.Entities.AssetReport>(paginatedResults, results.Count());

        return response;

    }

}
