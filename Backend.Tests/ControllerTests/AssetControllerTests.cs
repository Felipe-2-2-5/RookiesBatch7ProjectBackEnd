using Backend.API.Controllers;
using Backend.Application.Common.Paging;
using Backend.Application.DTOs.AssetDTOs;
using Backend.Application.Services.AssetServices;
using Backend.Application.Services.ReportServices;
using Backend.Domain.Entities;
using Backend.Domain.Enum;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Routing;
using Moq;
using System.Reflection;
using System.Security.Claims;

//asset
namespace Backend.Tests.Controllers
{
    [TestFixture]
    public class AssetsControllerTests
    {
        private Mock<IAssetService> _mockAssetService;
        private Mock<IReportService> _mockReportService;
        private AssetsController _controller;

        [SetUp]
        public void SetUp()
        {
            _mockAssetService = new Mock<IAssetService>();
            _mockReportService = new Mock<IReportService>();

            // Mock HttpContext and User claims
            var httpContext = new DefaultHttpContext();
            httpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
            {
                new Claim("UserName", "testuser"),
                new Claim("Location", Location.HaNoi.ToString()),
                new Claim("UserId", "1"),
                new Claim(ClaimTypes.Role, Role.Admin.ToString())
            }, "mock"));

            // Setup ControllerActionDescriptor
            var controllerActionDescriptor = new ControllerActionDescriptor
            {
                ControllerTypeInfo = typeof(ReturnRequestsController).GetTypeInfo(),
                MethodInfo = typeof(ReturnRequestsController).GetMethod("GetFilterAsync"),
                //ParameterDescriptors = new List<ParameterDescriptor>(), 
            };

            var actionContext = new ActionContext(httpContext, new RouteData(), controllerActionDescriptor);

            _controller = new AssetsController(_mockAssetService.Object, _mockReportService.Object)
            {
                ControllerContext = new ControllerContext(actionContext)
            };
        }

        [Test]
        public async Task InsertAsync_WithValidDto_ShouldReturnOkResult()
        {
            // Arrange
            var assetDto = new AssetDTO();
            _mockAssetService.Setup(s => s.InsertAsync(assetDto, It.IsAny<string>(), It.IsAny<Location>()))
                             .ReturnsAsync(new AssetResponse());

            // Act
            var result = await _controller.InsertAsync(assetDto);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.That(okResult.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
        }


        [Test]
        public async Task UpdateAsync_ShouldReturnOkResult()
        {
            // Arrange
            var dto = new AssetDTO();
            _mockAssetService.Setup(s => s.UpdateAsync(It.IsAny<int>(), It.IsAny<AssetDTO>(), It.IsAny<string>()))
                             .ReturnsAsync(new AssetResponse());

            // Act
            var result = await _controller.UpdateAsync(1, dto);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]

        public async Task GetByIdAsync_ShouldReturnOkResult()
        {
            // Arrange
            _mockAssetService.Setup(s => s.GetByIdAsync(It.IsAny<int>()))
                             .ReturnsAsync(new AssetResponse());

            // Act
            var result = await _controller.GetByIdAsync(1);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
        }


        [Test]
        public async Task GetFilterAsync_ShouldReturnOkResult()
        {
            // Arrange
            var request = new AssetFilterRequest();
            var paginationResponse = new PaginationResponse<AssetResponse>(new List<AssetResponse> { new AssetResponse() }, 1);
            _mockAssetService.Setup(s => s.GetFilterAsync(It.IsAny<AssetFilterRequest>(), It.IsAny<Location>()))
                             .ReturnsAsync(paginationResponse);

            // Act
            var result = await _controller.GetFilterAsync(request);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task DeleteAsync_ShouldReturnNoContentResult()
        {
            // Arrange
            _mockAssetService.Setup(s => s.DeleteAsync(It.IsAny<int>()))
                             .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteAsync(1);

            // Assert
            Assert.IsInstanceOf<NoContentResult>(result);
        }


        [Test]
        public async Task GetAssetReport_ShouldReturnOkResult()
        {
            // Arrange
            var filterDto = new BaseFilterRequest();
            var paginationResponse = new PaginationResponse<AssetReport>(new List<AssetReport> { new AssetReport() }, 1);
            _mockReportService.Setup(s => s.GetAssetReportAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
                              .ReturnsAsync(paginationResponse);

            // Act
            var result = await _controller.GetAssetReport(filterDto);

            // Assert
            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);

        }


        [Test]
        public async Task ExportAssetReport_ShouldReturnFileResult()
        {
            var filterDto = new BaseFilterRequest();
            // Arrange
            _mockReportService.Setup(s => s.ExportAssetReportAsync(It.IsAny<string>(), It.IsAny<string>()))
                              .ReturnsAsync(new byte[0]);

            // Act
            var result = await _controller.ExportAssetReport(filterDto);

            // Assert
            Assert.IsInstanceOf<FileContentResult>(result);
        }
    }
}
