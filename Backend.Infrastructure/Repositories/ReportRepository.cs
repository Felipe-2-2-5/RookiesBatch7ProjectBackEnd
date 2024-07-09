using Backend.Application.Common.Paging;
using Backend.Application.DTOs.AssetDTOs;
using Backend.Application.IRepositories;
using Backend.Infrastructure.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

public class ReportRepository : IReportRepository
{
    private readonly AssetContext _context;
    private readonly string _connectionString;

    public ReportRepository(AssetContext context)
    {
        _context = context;
        _connectionString = context.Database.GetConnectionString()!;
    }

    public async Task<PaginationResponse<AssetReportDto>> GetAssetReportAsync(string? SortColumn, string? SortDirection, int? PageSize, int? Page)
    {
        var sortColumnParameter = new SqlParameter("@SortColumn", SortColumn ?? "Category");
        var sortDirectionParameter = new SqlParameter("@SortDirection", SortDirection ?? "ASC");

        var sqlCommand = "getAssetReportByCategoryAndState";

        // Log connection string for debugging (redact sensitive information)
        Console.WriteLine($"ConnectionString: {_connectionString.Substring(0, _connectionString.IndexOf(';', _connectionString.IndexOf(';') + 1))}...");

        using (var connection = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
        {
            try
            {
                await connection.OpenAsync();
                var results = (await connection.QueryAsync<AssetReportDto>(sqlCommand, new
                {
                    SortColumn = sortColumnParameter.Value,
                    SortDirection = sortDirectionParameter.Value
                }, commandType: CommandType.StoredProcedure)).ToList();

                var totalCount = results.Count;

                if (PageSize.HasValue && Page.HasValue)
                {
                    var pagedResults = results
                        .Skip((Page.Value - 1) * PageSize.Value)
                        .Take(PageSize.Value)
                        .ToList();

                    return new PaginationResponse<AssetReportDto>(pagedResults, totalCount);
                }
                else
                {
                    return new PaginationResponse<AssetReportDto>(results, totalCount);
                }
            }
            catch (SqlException ex)
            {
                // Log SQL exception details for debugging
                Console.WriteLine($"SqlException: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                throw;
            }
        }
    }
}
