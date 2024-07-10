using Backend.Application.Common.Paging;
using Backend.Application.IRepositories;
using Backend.Domain.Entities;
using ClosedXML.Excel;


namespace Backend.Application.Services.ReportServices
{
    public class ReportService : IReportService
    {
        private readonly IReportRepository _reportRepository;

        public ReportService(IReportRepository reportRepository)
        {
            _reportRepository = reportRepository;
        }

        //get report by category and state 
        public async Task<PaginationResponse<AssetReport>> GetAssetReportAsync(string? SortColumn, string? SortDirection, int? PageSize, int? Page)
        {
            return await _reportRepository.GetAssetReportAsync(SortColumn, SortDirection, PageSize, Page);
        }

        public async Task<byte[]> ExportAssetReportAsync(string? SortColumn, string? SortOrder)
        {
            // Get all results without pagination
            var results = await _reportRepository.GetAssetReportAsync(SortColumn, SortOrder, null, null);

            if (results.Data.Count() == 0)
            {
                return null!; // Return null if there are no results
            }

            using (XLWorkbook workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("AssetReport");

                // Header style 
                var headerStyle = workbook.Style;
                headerStyle.Font.Bold = true;
                headerStyle.Font.FontColor = XLColor.White;
                headerStyle.Fill.BackgroundColor = XLColor.FromArgb(192, 80, 77);
                headerStyle.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                headerStyle.Border.TopBorder = XLBorderStyleValues.Thin;
                headerStyle.Border.BottomBorder = XLBorderStyleValues.Thin;
                headerStyle.Border.LeftBorder = XLBorderStyleValues.Thin;
                headerStyle.Border.RightBorder = XLBorderStyleValues.Thin;
                headerStyle.Border.TopBorderColor = XLColor.Black;
                headerStyle.Border.BottomBorderColor = XLColor.Black;
                headerStyle.Border.LeftBorderColor = XLColor.Black;
                headerStyle.Border.RightBorderColor = XLColor.Black;

                // Add headers and apply styles
                var properties = typeof(AssetReport).GetProperties();
                for (int i = 0; i < properties.Length; i++)
                {
                    var cell = worksheet.Cell(1, i + 1);
                    cell.Value = properties[i].Name;
                    cell.Style = headerStyle;
                }

                // Add rows and center all data
                for (int i = 0; i < results.Data.Count(); i++)
                {
                    var result = results.Data.ElementAt(i);
                    for (int j = 0; j < properties.Length; j++)
                    {
                        var cell = worksheet.Cell(i + 2, j + 1);
                        var value = properties[j].GetValue(result)?.ToString();
                        if (double.TryParse(value, out double number))
                        {
                            cell.Value = number;
                            cell.Style.NumberFormat.Format = "#,##0";
                        }
                        else
                        {
                            cell.Value = value;
                        }
                    }
                }
                // Auto-fit columns to content
                worksheet.Columns().AdjustToContents();

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    return stream.ToArray();
                }
            }
        }
    }
}
