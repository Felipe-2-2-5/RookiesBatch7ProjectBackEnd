using Backend.Application.DTOs.AssetDTOs;
using Backend.Application.IRepositories;
using Backend.Infrastructure.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Backend.Application.Common.Paging;
using Backend.Domain.Entities;

public class ReportRepository : IReportRepository
{
    private readonly AssetContext _context;
    private readonly string _connectionString;
    public ReportRepository(AssetContext context)
    {
        _context = context;
        _connectionString = context.Database.GetConnectionString();

    }


    //public async Task<List<AssetReportDto>> GetAssetReportAsync(string sortColumn, string sortDirection)
    //{
    //    var sortColumnParameter = new SqlParameter("@SortColumn", sortColumn ?? "Category");
    //    var sortDirectionParameter = new SqlParameter("@SortDirection", sortDirection ?? "ASC");

    //    var sqlCommand = "getAssetReportByCategoryAndState";

    //    // Execute raw SQL query with Dapper
    //    using (var connection = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
    //    {
    //        connection.Open();
    //        var results = await connection.QueryAsync<AssetReportDto>(sqlCommand, new { SortColumn = sortColumnParameter.Value, SortDirection = sortDirectionParameter.Value }, commandType: CommandType.StoredProcedure);
    //        return  results.AsList();
    //    }
    //}
    public async Task<PaginationResponse<AssetReportDto>> GetAssetReportAsync(string SortColumn, string SortDirection, int PageSize, int Page)
    {
        var sortColumnParameter = new SqlParameter("@SortColumn", SortColumn ?? "Category");
        var sortDirectionParameter = new SqlParameter("@SortDirection", SortDirection ?? "ASC");

        var sqlCommand = "getAssetReportByCategoryAndState";

        using (var connection = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
        {
            connection.Open();
            var results = (await connection.QueryAsync<AssetReportDto>(sqlCommand, new
            {
                SortColumn = sortColumnParameter.Value,
                SortDirection = sortDirectionParameter.Value
            }, commandType: CommandType.StoredProcedure)).ToList();

            var totalCount = results.Count;
            // pagination
            var pagedResults = results
                .Skip((Page - 1) * PageSize)
                .Take(PageSize)
                .ToList();

            //return pagedResults;
            return new PaginationResponse<AssetReportDto>(pagedResults, totalCount);
        }
    }


}
