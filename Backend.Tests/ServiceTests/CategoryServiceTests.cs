using AutoMapper;
using Backend.Application.DTOs.CategoryDTOs;
using Backend.Application.IRepositories;
using Backend.Application.Services.CategoryServices;
using Backend.Domain.Entity;
using Backend.Domain.Exceptions;
using FluentValidation;
using FluentValidation.Results;
using Moq;

namespace Backend.Tests.ServiceTests
{
    [TestFixture]
    public class CategoryServiceTests
    {
        private Mock<ICategoryRepository> _categoryRepositoryMock;
        private Mock<IMapper> _mapperMock;
        private Mock<IValidator<CategoryDTO>> _validatorMock;
        private CategoryService _categoryService;

        [SetUp]
        public void SetUp()
        {
            _categoryRepositoryMock = new Mock<ICategoryRepository>();
            _mapperMock = new Mock<IMapper>();
            _validatorMock = new Mock<IValidator<CategoryDTO>>();
            _categoryService = new CategoryService(_categoryRepositoryMock.Object, _mapperMock.Object, _validatorMock.Object);
        }
        [Test]
        public async Task InsertAsync_WhenValid_ShouldInsertCategoryAndReturnResponse()
        {
            // Arrange
            var dto = new CategoryDTO { Prefix = "test", Name = "name" };
            var validationResult = new ValidationResult();
            var category = new Category();
            var categoryResponse = new CategoryResponse();

            _validatorMock.Setup(v => v.ValidateAsync(dto, default)).ReturnsAsync(validationResult);
            _categoryRepositoryMock.SetupSequence(r => r.FindCategoryByPrefixAsync(dto.Prefix))
                                   .ReturnsAsync((Category?)null) // First call returns null
                                   .ReturnsAsync(category);      // Second call returns the category
            _categoryRepositoryMock.Setup(r => r.FindCategoryByNameAsync(dto.Name)).ReturnsAsync((Category?)null);
            _mapperMock.Setup(m => m.Map<Category>(dto)).Returns(category);
            _categoryRepositoryMock.Setup(r => r.InsertAsync(category)).Returns(Task.CompletedTask);
            _mapperMock.Setup(m => m.Map<CategoryResponse>(category)).Returns(categoryResponse);

            // Act
            var result = await _categoryService.InsertAsync(dto);

            // Assert
            Assert.That(result, Is.EqualTo(categoryResponse));
        }

        [Test]
        public void InsertAsync_WhenCategoryIsInvalid_ThrowsDataInvalidException()
        {
            // Arrange
            var categoryDto = new CategoryDTO { Name = "TestCategory", Prefix = "TC" };
            var validationResult = new ValidationResult(new List<ValidationFailure>
            {
                new ValidationFailure("Name", "Error message")
            });

            _validatorMock.Setup(v => v.ValidateAsync(categoryDto, default)).ReturnsAsync(validationResult);

            // Act & Assert
            var ex = Assert.ThrowsAsync<DataInvalidException>(() => _categoryService.InsertAsync(categoryDto));
            Assert.That(ex.Message, Is.EqualTo("Error message"));
        }

        [Test]
        public void InsertAsync_WhenCategoryNameExists_ThrowsDataInvalidException()
        {
            // Arrange
            var categoryDto = new CategoryDTO { Name = "TestCategory", Prefix = "TC" };
            var validationResult = new ValidationResult();
            var existingCategory = new Category { Name = "TestCategory" };

            _validatorMock.Setup(v => v.ValidateAsync(categoryDto, default)).ReturnsAsync(validationResult);
            _categoryRepositoryMock.Setup(r => r.FindCategoryByNameAsync(categoryDto.Name)).ReturnsAsync(existingCategory);

            // Act & Assert
            var ex = Assert.ThrowsAsync<DataInvalidException>(() => _categoryService.InsertAsync(categoryDto));
            Assert.That(ex.Message, Is.EqualTo("Existing category name"));
        }

        [Test]
        public void InsertAsync_WhenCategoryPrefixExists_ThrowsDataInvalidException()
        {
            // Arrange
            var categoryDto = new CategoryDTO { Name = "TestCategory", Prefix = "TC" };
            var validationResult = new ValidationResult();
            var existingCategory = new Category { Prefix = "TC" };

            _validatorMock.Setup(v => v.ValidateAsync(categoryDto, default)).ReturnsAsync(validationResult);
            _categoryRepositoryMock.Setup(r => r.FindCategoryByPrefixAsync(categoryDto.Prefix)).ReturnsAsync(existingCategory);

            // Act & Assert
            var ex = Assert.ThrowsAsync<DataInvalidException>(() => _categoryService.InsertAsync(categoryDto));
            Assert.That(ex.Message, Is.EqualTo("Existing category prefix"));
        }

        [Test]
        public async Task GetFilterAsync_ReturnsFilteredCategories()
        {
            // Arrange
            var searchTerm = "Test";
            var categories = new List<Category> { new Category { Name = "TestCategory" } };
            var categoryResponses = new List<CategoryResponse> { new CategoryResponse { Name = "TestCategory" } };

            _categoryRepositoryMock.Setup(r => r.GetFilterAsync(searchTerm)).ReturnsAsync(categories);
            _mapperMock.Setup(m => m.Map<IEnumerable<CategoryResponse>>(categories)).Returns(categoryResponses);

            // Act
            var result = await _categoryService.GetFilterAsync(searchTerm);

            // Assert
            Assert.That(result, Is.EqualTo(categoryResponses));
        }

        [Test]
        public async Task GetAllAsync_ReturnsAllCategories()
        {
            // Arrange
            var categories = new List<Category> { new Category { Name = "TestCategory" } };
            var categoryResponses = new List<CategoryResponse> { new CategoryResponse { Name = "TestCategory" } };

            _categoryRepositoryMock.Setup(r => r.GetAllCategoriesAsync()).ReturnsAsync(categories);
            _mapperMock.Setup(m => m.Map<IEnumerable<CategoryResponse>>(categories)).Returns(categoryResponses);

            // Act
            var result = await _categoryService.GetAllAsync();

            // Assert
            Assert.That(result, Is.EqualTo(categoryResponses));
        }
    }
}
