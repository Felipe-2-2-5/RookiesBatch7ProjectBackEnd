using Backend.Application.Common.Paging;
using Backend.Application.IRepositories;
using Backend.Application.Services.ReportServices;
using Backend.Domain.Entities;
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
            ;
            var page = 1;

            var paginationResponse = new PaginationResponse<AssetReport>(new List<AssetReport>(), 0);

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
            var assetReports = new List<AssetReport>
            {
                new AssetReport {  }
            };

            var paginationResponse = new PaginationResponse<AssetReport>(assetReports, assetReports.Count);

            _reportRepositoryMock.Setup(repo => repo.GetAssetReportAsync(null, null, null, null))
                                 .ReturnsAsync(paginationResponse);

            // Act
            var result = await _reportService.ExportAssetReportAsync(It.IsAny<string>(), It.IsAny<string>());

            // Assert
            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public async Task ExportAssetReportAsync_ShouldReturnNullWhenNoData()
        {
            // Arrange
            var paginationResponse = new PaginationResponse<AssetReport>(new List<AssetReport>(), 0);

            _reportRepositoryMock.Setup(repo => repo.GetAssetReportAsync(null, null, null, null))
                                 .ReturnsAsync(paginationResponse);

            // Act
            var result = await _reportService.ExportAssetReportAsync(It.IsAny<string>(), It.IsAny<string>());

            // Assert
            Assert.That(result, Is.Null);
        }
    }
}
