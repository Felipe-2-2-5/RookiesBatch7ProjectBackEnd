using NUnit.Framework;
using Moq;
using System.Threading.Tasks;
using Backend.API.Controllers;
using Backend.Application.Services.CategoryServices;
using Microsoft.AspNetCore.Mvc;
using Backend.Application.DTOs.CategoryDTOs;
using System.Collections.Generic;
using Backend.Domain.Enum;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Backend.UnitTests.Controllers
{
    [TestFixture]
    public class CategoryControllerTests
    {
        private Mock<ICategoryService> _mockCategoryService;
        private CategoriesController _controller;

        [SetUp]
        public void Setup()
        {
            _mockCategoryService = new Mock<ICategoryService>();
            _controller = new CategoriesController(_mockCategoryService.Object);

            // Mock HttpContext and User
            _controller.ControllerContext = new ControllerContext();
            _controller.ControllerContext.HttpContext = new DefaultHttpContext();
            _controller.ControllerContext.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Role, Role.Admin.ToString())
            }));
        }

        [Test]
        public async Task GetFilterAsync_ReturnsOkResult_WithFilteredCategories()
        {
            // Arrange
            string searchTerm = "test";
            var expectedCategories = new List<CategoryResponse> { new CategoryResponse { Id = 1,Prefix = "TestPrefix",  Name = "TestCategory" } };

            _mockCategoryService.Setup(service => service.GetFilterAsync(searchTerm))
                .ReturnsAsync(expectedCategories);

            // Act
            var result = await _controller.GetFilterAsync(searchTerm);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(expectedCategories, okResult.Value);
        }

        [Test]
        public async Task InsertAsync_ReturnsOkResult_WithInsertedCategory()
        {
            // Arrange
            var categoryDto = new CategoryDTO { Prefix = "newPrefix", Name = "NewCategory" };
            var expectedCategory = new CategoryResponse { Id = 1, Prefix = "NewPrefix",  Name = "NewCategory" };

            _mockCategoryService.Setup(service => service.InsertAsync(categoryDto))
                .ReturnsAsync(expectedCategory);

            // Act
            var result = await _controller.InsertAsync(categoryDto);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(expectedCategory, okResult.Value);
        }

        [Test]
        public async Task GetAllAsync_ReturnsOkResult_WithAllCategories()
        {
            // Arrange
            var expectedCategories = new List<CategoryResponse>
            {
                new CategoryResponse { Id = 1, Name = "Category1" },
                new CategoryResponse { Id = 2, Name = "Category2" }
            };

            _mockCategoryService.Setup(service => service.GetAllAsync())
                .ReturnsAsync(expectedCategories);

            // Act
            var result = await _controller.GetAllAsync();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(expectedCategories, okResult.Value);
        }
    }
}
