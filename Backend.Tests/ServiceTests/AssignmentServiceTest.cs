using AutoMapper;
using Backend.Application.Common.Paging;
using Backend.Application.DTOs.AssignmentDTOs;
using Backend.Application.IRepositories;
using Backend.Application.Services.AssignmentServices;
using Backend.Domain.Entities;
using Backend.Domain.Enum;
using Backend.Domain.Exceptions;
using Moq;

namespace Backend.Tests.ServiceTests
{
    [TestFixture]
    public class AssignmentServiceTests
    {
        private Mock<IAssignmentRepository> _assignmentRepoMock;
        private Mock<IMapper> _mapperMock;
        private AssignmentService _assignmentService;
        private Mock<IAssetRepository> _assetRepoMock;
        private Mock<IUserRepository> _userRepoMock;

        [SetUp]
        public void SetUp()
        {
            _assignmentRepoMock = new Mock<IAssignmentRepository>();
            _assetRepoMock = new Mock<IAssetRepository>();
            _mapperMock = new Mock<IMapper>();
            _assetRepoMock = new Mock<IAssetRepository>();
            _userRepoMock = new Mock<IUserRepository>();
            _assignmentService = new AssignmentService(
                _assignmentRepoMock.Object,
                _mapperMock.Object,
                _assetRepoMock.Object,
                _userRepoMock.Object
            );
        }

        [Test]
        public async Task GetByIdAsync_WhenAssignmentExists_ReturnsAssignmentResponse()
        {
            // Arrange
            var assignmentId = 1;
            var assignment = new Assignment { Id = assignmentId };
            var assignmentResponse = new AssignmentResponse { Id = assignmentId };

            _assignmentRepoMock.Setup(repo => repo.GetByIdAsync(assignmentId)).ReturnsAsync(assignment);
            _mapperMock.Setup(mapper => mapper.Map<AssignmentResponse>(assignment)).Returns(assignmentResponse);

            // Act
            var result = await _assignmentService.GetByIdAsync(assignmentId);

            // Assert
            Assert.That(result, Is.EqualTo(assignmentResponse));
        }

        [Test]
        public void GetByIdAsync_WhenAssignmentDoesNotExist_ThrowsNotFoundException()
        {
            // Arrange
            var assignmentId = 1;

            _assignmentRepoMock.Setup(repo => repo.GetByIdAsync(assignmentId)).ReturnsAsync((Assignment)null);

            // Act & Assert
            Assert.ThrowsAsync<NotFoundException>(() => _assignmentService.GetByIdAsync(assignmentId));
        }

        [Test]
        public async Task InsertAsync_WhenAssignmentIsValid_ReturnsAssignmentResponse()
        {
            // Arrange
            var assignmentDto = new AssignmentDTO { AssetId = 3, AssignedToId = 1 };
            var assignment = new Assignment { AssetId = 3, AssignedToId = 1 };
            var assignmentResponse = new AssignmentResponse { AssetId = 3, AssignedToId = 1 };
            var asset = new Asset { Id = 3, State = AssetState.Available };

            // Mocking the mapper
            _mapperMock.Setup(mapper => mapper.Map<Assignment>(assignmentDto)).Returns(assignment);

            // Mocking the repository calls
            _assignmentRepoMock.Setup(repo => repo.FindAssignmentByAssetIdAsync(assignmentDto.AssetId)).ReturnsAsync((Assignment)null);
            _assignmentRepoMock.Setup(repo => repo.InsertAsync(It.IsAny<Assignment>())).Returns(Task.CompletedTask);
            _assignmentRepoMock.Setup(repo => repo.FindLastestAssignment()).ReturnsAsync(assignment);

            // Mocking the asset repository
            _assetRepoMock.Setup(repo => repo.GetByIdAsync(assignmentDto.AssetId)).ReturnsAsync(asset);
            _assetRepoMock.Setup(repo => repo.UpdateAsync(It.IsAny<Asset>())).Returns(Task.CompletedTask);

            // Mocking the mapper for response
            _mapperMock.Setup(mapper => mapper.Map<AssignmentResponse>(It.IsAny<Assignment>())).Returns(assignmentResponse);

            // Act
            var result = await _assignmentService.InsertAsync(assignmentDto, "creator", 1);

            // Assert
            Assert.That(result, Is.EqualTo(assignmentResponse));
            _assignmentRepoMock.Verify(repo => repo.InsertAsync(It.IsAny<Assignment>()), Times.Once);
            _assetRepoMock.Verify(repo => repo.UpdateAsync(It.IsAny<Asset>()), Times.Once);
        }

        [Test]
        public async Task GetFilterAsync_ReturnsPaginationResponse()
        {
            // Arrange
            var filterRequest = new AssignmentFilterRequest();
            var assignments = new List<Assignment> { new Assignment() };
            var paginationResponse = new PaginationResponse<Assignment>(assignments, 1);
            var assignmentResponses = new List<AssignmentResponse> { new AssignmentResponse() };

            _assignmentRepoMock.Setup(repo => repo.GetFilterAsync(filterRequest)).ReturnsAsync(paginationResponse);
            _mapperMock.Setup(mapper => mapper.Map<IEnumerable<AssignmentResponse>>(assignments)).Returns(assignmentResponses);

            // Act
            var result = await _assignmentService.GetFilterAsync(filterRequest);

            // Assert
            Assert.That(result.Data, Is.EqualTo(assignmentResponses));
            Assert.That(result.TotalCount, Is.EqualTo(1));
        }

        [Test]
        public async Task FindAssignmentByAssetIdAsync_WhenAssignmentExists_ReturnsAssignment()
        {
            // Arrange
            var assetId = 1;
            var assignment = new Assignment { AssetId = assetId };

            _assignmentRepoMock.Setup(repo => repo.FindAssignmentByAssetIdAsync(assetId)).ReturnsAsync(assignment);

            // Act
            var result = await _assignmentService.FindAssignmentByAssetIdAsync(assetId);

            // Assert
            Assert.That(result, Is.EqualTo(assignment));
        }

        [Test]
        public async Task FindAssignmentByAssetIdAsync_WhenAssignmentDoesNotExist_ReturnsNull()
        {
            // Arrange
            var assetId = 1;

            _assignmentRepoMock.Setup(repo => repo.FindAssignmentByAssetIdAsync(assetId)).ReturnsAsync((Assignment)null);

            // Act
            var result = await _assignmentService.FindAssignmentByAssetIdAsync(assetId);

            // Assert
            Assert.That(result, Is.Null);
        }
    }
}