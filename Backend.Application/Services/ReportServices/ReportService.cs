using Backend.Application.Common.Paging;
using Backend.Application.DTOs.AssetDTOs;
using Backend.Application.IRepositories;
using ClosedXML.Excel;


namespace Backend.Application.Services.ReportServices
{
    public class ReportService : IReportService
    {
        private readonly IReportRepository _reportRepository;

        public ReportService ( IReportRepository reportRepository)
        {
            _reportRepository = reportRepository;
        }

        //get report by category and state 
        public async Task<PaginationResponse<AssetReportDto>> GetAssetReportAsync(string SortColumn, string SortDirection, int PageSize, int Page)
        {
            return await _reportRepository.GetAssetReportAsync(SortColumn, SortDirection, PageSize, Page);
        }

        public async Task<string> ExportAssetReportAsync()
        {
            // Get all results without pagination
            var results = await _reportRepository.GetAssetReportAsync(null, null, null, null);

            using (XLWorkbook workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("AssetReport");

                // Add headers
                var properties = typeof(AssetReportDto).GetProperties();
                for (int i = 0; i < properties.Length; i++)
                {
                    worksheet.Cell(1, i + 1).Value = properties[i].Name;
                }

                // Add rows
                for (int i = 0; i < results.Data.Count(); i++)
                {
                    var result = results.Data.ElementAt(i);
                    for (int j = 0; j < properties.Length; j++)
                    {
                        var value = properties[j].GetValue(result);
                        worksheet.Cell(i + 2, j + 1).Value = value?.ToString();
                    }
                }

                // Include current date in file name
                var currentDate = DateTime.Now.ToString("ddMMyyyy");
                var filePath = $"AssetReport_{currentDate}_RookiesTeam2.xlsx";

                workbook.SaveAs(filePath);

                return filePath;
            }
        }
    }
}
