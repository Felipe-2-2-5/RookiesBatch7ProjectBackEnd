using AutoMapper;
using Backend.Application.Common.Paging;
using Backend.Application.DTOs.AssetDTOs;
using Backend.Application.IRepositories;
using Backend.Application.Services.AssetServices;
using Backend.Domain.Entities;
using Backend.Domain.Enum;
using Backend.Domain.Exceptions;
using FluentValidation;
using FluentValidation.Results;
using Moq;

namespace Backend.Application.Tests.Services
{
    [TestFixture]
    public class AssetServiceTests
    {
        private Mock<IAssetRepository> _assetRepositoryMock;
        private Mock<IMapper> _mapperMock;
        private Mock<IValidator<AssetDTO>> _validatorMock;
        private AssetService _assetService;

        [SetUp]
        public void Setup()
        {
            _assetRepositoryMock = new Mock<IAssetRepository>();
            _mapperMock = new Mock<IMapper>();
            _validatorMock = new Mock<IValidator<AssetDTO>>();
            _assetService = new AssetService(_assetRepositoryMock.Object, _mapperMock.Object, _validatorMock.Object);
        }
        [Test]
        public async Task GetFilterAsync_ReturnsPaginationResponse()
        {
            // Arrange
            var filterRequest = new AssetFilterRequest();
            var assets = new List<Asset> { new Asset() };
            var paginationResponse = new PaginationResponse<Asset>(assets, 1);
            var assetResponses = new List<AssetResponse> { new AssetResponse() };

            _assetRepositoryMock.Setup(repo => repo.GetFilterAsync(filterRequest, Location.HaNoi)).ReturnsAsync(paginationResponse);
            _mapperMock.Setup(mapper => mapper.Map<IEnumerable<AssetResponse>>(assets)).Returns(assetResponses);

            // Act
            var result = await _assetService.GetFilterAsync(filterRequest, Location.HaNoi);

            // Assert
            Assert.That(result.Data, Is.EqualTo(assetResponses));
            Assert.That(result.TotalCount, Is.EqualTo(1));
        }
        [Test]
        public async Task InsertAsync_ValidAssetDTO_ShouldReturnAssetResponse()
        {
            // Arrange
            var assetDTO = new AssetDTO();
            var validationResult = new ValidationResult();
            var asset = new Asset { AssetCode = "123" };
            var assetResponse = new AssetResponse();

            _validatorMock.Setup(v => v.ValidateAsync(assetDTO, default)).ReturnsAsync(validationResult);
            _mapperMock.Setup(m => m.Map<Asset>(assetDTO)).Returns(asset);
            _mapperMock.Setup(m => m.Map<AssetResponse>(It.IsAny<Asset>())).Returns(assetResponse);
            /*            _assetRepositoryMock.Setup(r => r.GenerateAssetInfo(It.IsAny<Asset>())).Returns(asset);
            */
            _assetRepositoryMock.Setup(r => r.InsertAsync(It.IsAny<Asset>())).Returns(Task.CompletedTask);
            _assetRepositoryMock.Setup(r => r.FindAssetByCodeAsync(It.IsAny<string>())).ReturnsAsync(asset);

            // Act
            var result = await _assetService.InsertAsync(assetDTO, "testUser", Location.HaNoi);

            // Assert
            Assert.That(result, Is.EqualTo(assetResponse));
        }

        [Test]
        public void InsertAsync_InvalidAssetDTO_ShouldThrowDataInvalidException()
        {
            // Arrange
            var assetDTO = new AssetDTO();
            var validationResult = new ValidationResult(new List<ValidationFailure> { new ValidationFailure("Error", "Invalid data") });

            _validatorMock.Setup(v => v.ValidateAsync(assetDTO, default)).ReturnsAsync(validationResult);

            // Act & Assert
            Assert.ThrowsAsync<DataInvalidException>(() => _assetService.InsertAsync(assetDTO, "testUser", Location.HaNoi));
        }

        [Test]
        public async Task UpdateAsync_ValidAssetDTO_ShouldReturnAssetResponse()
        {
            // Arrange
            var assetDTO = new AssetDTO
            {
                AssetName = "Test Asset",
                Specification = "Test Specification",
                InstalledDate = DateTime.Now,
                State = AssetState.Available
            };
            var validationResult = new ValidationResult();
            var asset = new Asset();
            var assetResponse = new AssetResponse();

            _validatorMock.Setup(v => v.ValidateAsync(assetDTO, default)).ReturnsAsync(validationResult);
            _assetRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(asset);
            _mapperMock.Setup(m => m.Map<AssetResponse>(It.IsAny<Asset>())).Returns(assetResponse);

            // Act
            var result = await _assetService.UpdateAsync(1, assetDTO, "testUser");

            // Assert
            Assert.That(result, Is.EqualTo(assetResponse));
        }

        [Test]
        public void UpdateAsync_InvalidAssetDTO_ShouldThrowDataInvalidException()
        {
            // Arrange
            var assetDTO = new AssetDTO();
            var validationResult = new ValidationResult(new List<ValidationFailure> { new ValidationFailure("Error", "Invalid data") });

            _validatorMock.Setup(v => v.ValidateAsync(assetDTO, default)).ReturnsAsync(validationResult);

            // Act & Assert
            Assert.ThrowsAsync<DataInvalidException>(() => _assetService.UpdateAsync(1, assetDTO, "testUser"));
        }

        [Test]
        public void UpdateAsync_AssetNotFound_ShouldThrowNotFoundException()
        {
            // Arrange
            var assetDTO = new AssetDTO();
            var validationResult = new ValidationResult();

            _validatorMock.Setup(v => v.ValidateAsync(assetDTO, default)).ReturnsAsync(validationResult);
            _assetRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Asset)null);

            // Act & Assert
            Assert.ThrowsAsync<NotFoundException>(() => _assetService.UpdateAsync(1, assetDTO, "testUser"));
        }

        [Test]
        public async Task GetByIdAsync_ValidId_ShouldReturnAssetResponse()
        {
            // Arrange
            var asset = new Asset();
            var assetResponse = new AssetResponse();

            _assetRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(asset);
            _mapperMock.Setup(m => m.Map<AssetResponse>(It.IsAny<Asset>())).Returns(assetResponse);

            // Act
            var result = await _assetService.GetByIdAsync(1);

            // Assert
            Assert.That(result, Is.EqualTo(assetResponse));
        }

        [Test]
        public void GetByIdAsync_AssetNotFound_ShouldThrowNotFoundException()
        {
            // Arrange
            _assetRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Asset)null);

            // Act & Assert
            Assert.ThrowsAsync<NotFoundException>(() => _assetService.GetByIdAsync(1));
        }

        [Test]
        public async Task DeleteAsync_ValidId_ShouldCallRepositoryDelete()
        {
            // Arrange
            var asset = new Asset { State = AssetState.Available, Assignments = null };

            _assetRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(asset);
            _assetRepositoryMock.Setup(r => r.DeleteAsync(It.IsAny<Asset>())).Returns(Task.CompletedTask);

            // Act
            await _assetService.DeleteAsync(1);

            // Assert
            _assetRepositoryMock.Verify(r => r.DeleteAsync(It.IsAny<Asset>()), Times.Once);
        }

        [Test]
        public void DeleteAsync_AssetNotFound_ShouldThrowNotFoundException()
        {
            // Arrange
            _assetRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Asset)null);

            // Act & Assert
            Assert.ThrowsAsync<NotFoundException>(() => _assetService.DeleteAsync(1));
        }

        [Test]
        public void DeleteAsync_AssetAssigned_ShouldThrowDataInvalidException()
        {
            // Arrange
            var asset = new Asset { State = AssetState.Assigned };

            _assetRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(asset);

            // Act & Assert
            Assert.ThrowsAsync<DataInvalidException>(() => _assetService.DeleteAsync(1));
        }

        [Test]
        public void DeleteAsync_AssetHasAssignments_ShouldThrowDataInvalidException()
        {
            // Arrange
            var asset = new Asset { Assignments = new List<Assignment> { new Assignment() } };

            _assetRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(asset);

            // Act & Assert
            Assert.ThrowsAsync<DataInvalidException>(() => _assetService.DeleteAsync(1));
        }
    }
}
