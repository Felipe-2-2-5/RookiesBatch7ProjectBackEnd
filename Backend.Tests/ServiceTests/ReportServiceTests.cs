using Backend.Application.Common.Paging;
using Backend.Application.DTOs.AssetDTOs;
using Backend.Application.IRepositories;
using Backend.Application.Services.ReportServices;
using ClosedXML.Excel;
using Moq;

namespace Backend.Application.Tests.Services.ReportServices
{
    [TestFixture]
    public class ReportServiceTests
    {
        private Mock<IReportRepository> _reportRepositoryMock;
        private ReportService _reportService;

        [SetUp]
        public void SetUp()
        {
            _reportRepositoryMock = new Mock<IReportRepository>();
            _reportService = new ReportService(_reportRepositoryMock.Object);
        }

        [Test]
        public async Task GetAssetReportAsync_ShouldCallRepositoryMethod()
        {
            // Arrange
            var sortColumn = "Name";
            var sortDirection = "asc";
            var pageSize = 10;
            var page = 1;

            var paginationResponse = new PaginationResponse<AssetReportDto>(new List<AssetReportDto>(), 0);

            _reportRepositoryMock.Setup(repo => repo.GetAssetReportAsync(sortColumn, sortDirection, pageSize, page))
                                 .ReturnsAsync(paginationResponse);

            // Act
            var result = await _reportService.GetAssetReportAsync(sortColumn, sortDirection, pageSize, page);

            // Assert
            _reportRepositoryMock.Verify(repo => repo.GetAssetReportAsync(sortColumn, sortDirection, pageSize, page), Times.Once);
            Assert.That(result, Is.EqualTo(paginationResponse));
        }

        [Test]
        public async Task ExportAssetReportAsync_ShouldReturnExcelFile()
        {
            // Arrange
            var assetReports = new List<AssetReportDto>
            {
                new AssetReportDto { /* Set properties as needed */ }
            };

            var paginationResponse = new PaginationResponse<AssetReportDto>(assetReports, assetReports.Count);

            _reportRepositoryMock.Setup(repo => repo.GetAssetReportAsync(null, null, null, null))
                                 .ReturnsAsync(paginationResponse);

            // Act
            var result = await _reportService.ExportAssetReportAsync();

            // Assert
            Assert.That(result, Is.Not.Null);
            using var stream = new MemoryStream(result);
            using var workbook = new XLWorkbook(stream);
            var worksheet = workbook.Worksheets.First();
            var properties = typeof(AssetReportDto).GetProperties();

            // Check headers
            for (int i = 0; i < properties.Length; i++)
            {
                Assert.That(worksheet.Cell(1, i + 1).Value.ToString(), Is.EqualTo(properties[i].Name));
            }

            // Check data
            for (int i = 0; i < assetReports.Count; i++)
            {
                var report = assetReports[i];
                for (int j = 0; j < properties.Length; j++)
                {
                    var value = properties[j].GetValue(report)?.ToString();
                    Assert.That(worksheet.Cell(i + 2, j + 1).Value.ToString(), Is.EqualTo(value));
                }
            }
        }

        [Test]
        public async Task ExportAssetReportAsync_ShouldReturnNullWhenNoData()
        {
            // Arrange
            var paginationResponse = new PaginationResponse<AssetReportDto>(new List<AssetReportDto>(), 0);

            _reportRepositoryMock.Setup(repo => repo.GetAssetReportAsync(null, null, null, null))
                                 .ReturnsAsync(paginationResponse);

            // Act
            var result = await _reportService.ExportAssetReportAsync();

            // Assert
            Assert.That(result, Is.Null);
        }
    }
}
