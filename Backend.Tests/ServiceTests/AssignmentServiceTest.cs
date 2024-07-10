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

            _assignmentRepoMock.Setup(repo => repo.GetByIdAsync(assignmentId)).ReturnsAsync((Assignment?)null);

            // Act & Assert
            Assert.ThrowsAsync<NotFoundException>(() => _assignmentService.GetByIdAsync(assignmentId));
        }

        [Test]
        public async Task GetFilterAsync_WhenRepositoryReturnsData_ReturnsExpectedResult()
        {
            // Arrange
            var request = new AssignmentFilterRequest { };
            var location = new Location { };
            var assignments = new List<Assignment> { };
            var response = new PaginationResponse<Assignment>(assignments, assignments.Count);

            _assignmentRepoMock.Setup(repo => repo.GetFilterAsync(request, location))
                               .ReturnsAsync(response);
            _mapperMock.Setup(mapper => mapper.Map<IEnumerable<AssignmentResponse>>(assignments))
                       .Returns(new List<AssignmentResponse> { });

            // Act
            var result = await _assignmentService.GetFilterAsync(request, location);

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.TotalCount, Is.EqualTo(assignments.Count));
            _assignmentRepoMock.Verify(repo => repo.GetFilterAsync(request, location), Times.Once);
            _mapperMock.Verify(mapper => mapper.Map<IEnumerable<AssignmentResponse>>(assignments), Times.Once);
        }

        [Test]
        public async Task GetFilterAsync_WhenRepositoryReturnsNoData_ReturnsEmptyResult()
        {
            // Arrange
            var request = new AssignmentFilterRequest { /* Initialize properties */ };
            var location = new Location { /* Initialize properties */ };
            var response = new PaginationResponse<Assignment>(new List<Assignment>(), 0);

            _assignmentRepoMock.Setup(repo => repo.GetFilterAsync(request, location))
                               .ReturnsAsync(response);
            _mapperMock.Setup(mapper => mapper.Map<IEnumerable<AssignmentResponse>>(It.IsAny<IEnumerable<Assignment>>()))
                       .Returns(new List<AssignmentResponse>());

            // Act
            var result = await _assignmentService.GetFilterAsync(request, location);

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.TotalCount, Is.EqualTo(0));
            _assignmentRepoMock.Verify(repo => repo.GetFilterAsync(request, location), Times.Once);
            _mapperMock.Verify(mapper => mapper.Map<IEnumerable<AssignmentResponse>>(It.IsAny<IEnumerable<Assignment>>()), Times.Once);
        }

        [Test]
        public void GetFilterAsync_WhenRepositoryThrowsException_ThrowsException()
        {
            // Arrange
            var request = new AssignmentFilterRequest { /* Initialize properties */ };
            var location = new Location { /* Initialize properties */ };

            _assignmentRepoMock.Setup(repo => repo.GetFilterAsync(request, location))
                               .ThrowsAsync(new System.Exception("Test exception"));

            // Act & Assert
            Assert.ThrowsAsync<System.Exception>(async () => await _assignmentService.GetFilterAsync(request, location));
            _assignmentRepoMock.Verify(repo => repo.GetFilterAsync(request, location), Times.Once);
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
            _assignmentRepoMock.Setup(repo => repo.FindAssignmentByAssetIdAsync(assignmentDto.AssetId)).ReturnsAsync((Assignment?)null);
            _assignmentRepoMock.Setup(repo => repo.InsertAsync(It.IsAny<Assignment>())).Returns(Task.CompletedTask);
            _assignmentRepoMock.Setup(repo => repo.FindLatestAssignment()).ReturnsAsync(assignment);

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
        public void InsertAsync_WhenAssetAlreadyAssigned_ThrowsDataInvalidException()
        {
            // Arrange
            var assignmentDto = new AssignmentDTO { AssetId = 3, AssignedToId = 1 };
            var assignedAsset = new Asset { Id = 3, State = AssetState.Assigned };
            var existingAssignment = new Assignment { AssetId = 3, AssignedToId = 2 };
            var assignedUser = new User { Id = 2, UserName = "existingUser" };

            // Mocking the repositories
            _assetRepoMock.Setup(repo => repo.GetByIdAsync(assignmentDto.AssetId)).ReturnsAsync(assignedAsset);
            _assignmentRepoMock.Setup(repo => repo.FindAssignmentByAssetIdAsync(assignmentDto.AssetId)).ReturnsAsync(existingAssignment);
            _userRepoMock.Setup(repo => repo.GetByIdAsync(existingAssignment.AssignedToId)).ReturnsAsync(assignedUser);

            // Act
            var exception = Assert.ThrowsAsync<Exception>(() => _assignmentService.InsertAsync(assignmentDto, "creator", 1));

            // Assert
            Assert.That(exception.Message, Is.EqualTo("Error Asset has been assigned to existingUser "));
            Assert.That(exception.InnerException, Is.InstanceOf<DataInvalidException>());
            Assert.That(exception.InnerException?.Message, Is.EqualTo("Asset has been assigned to existingUser "));

            // Verifying that InsertAsync and UpdateAsync methods were never called
            _assignmentRepoMock.Verify(repo => repo.InsertAsync(It.IsAny<Assignment>()), Times.Never);
            _assetRepoMock.Verify(repo => repo.UpdateAsync(It.IsAny<Asset>()), Times.Never);
        }


        [Test]
        public void InsertAsync_WhenExceptionThrown_RethrowsException()
        {
            // Arrange
            var assignmentDto = new AssignmentDTO { AssetId = 3, AssignedToId = 1 };
            var asset = new Asset { Id = 3, State = AssetState.Available };

            // Mocking the asset repository to return a valid asset
            _assetRepoMock.Setup(repo => repo.GetByIdAsync(assignmentDto.AssetId)).ReturnsAsync(asset);

            // Mocking the assignment repository to throw an exception
            _assignmentRepoMock.Setup(repo => repo.InsertAsync(It.IsAny<Assignment>())).ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            var ex = Assert.ThrowsAsync<Exception>(() => _assignmentService.InsertAsync(assignmentDto, "creator", 1));
            Assert.That(ex.Message, Is.EqualTo("Error Object reference not set to an instance of an object."));

            // Verifying that UpdateAsync method was never called
            _assetRepoMock.Verify(repo => repo.UpdateAsync(It.IsAny<Asset>()), Times.Never);
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

            _assignmentRepoMock.Setup(repo => repo.FindAssignmentByAssetIdAsync(assetId)).ReturnsAsync((Assignment?)null);

            // Act
            var result = await _assignmentService.FindAssignmentByAssetIdAsync(assetId);

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task GetMyAssignmentsAsync_ReturnsCorrectPaginationResponse()
        {
            // Arrange
            var request = new MyAssignmentFilterRequest();
            var assignments = new List<Assignment> { new Assignment() };
            var paginationResponse = new PaginationResponse<Assignment>(assignments, 1);
            var assignmentResponses = new List<AssignmentResponse> { new AssignmentResponse() };

            _assignmentRepoMock.Setup(repo => repo.GetMyAssignmentsAsync(request)).ReturnsAsync(paginationResponse);
            _mapperMock.Setup(mapper => mapper.Map<IEnumerable<AssignmentResponse>>(assignments)).Returns(assignmentResponses);

            // Act
            var result = await _assignmentService.GetMyAssignmentsAsync(request);

            // Assert
            Assert.That(result.Data, Is.EqualTo(assignmentResponses));
            Assert.That(result.TotalCount, Is.EqualTo(1));
        }

        [Test]
        public async Task UpdateAsync_NoChanges_DoesNotUpdate()
        {
            // Arrange
            var dto = new AssignmentDTO { AssetId = 1, AssignedToId = 1 };
            int assignmentId = 1;
            string modifiedName = "testUser";
            var assignment = new Assignment { AssetId = 1, AssignedToId = 1, State = AssignmentState.WaitingForAcceptance };
            var oldAsset = new Asset { State = AssetState.Assigned, Assignments = new List<Assignment>() };

            _assignmentRepoMock.Setup(repo => repo.FindAssignmentByIdWithoutAsset(assignmentId))
                .ReturnsAsync(assignment);
            _assetRepoMock.Setup(repo => repo.GetByIdAsync(assignment.AssetId))
                .ReturnsAsync(oldAsset);

            // Act
            var response = await _assignmentService.UpdateAsync(dto, assignmentId, modifiedName);

            // Assert
            _assetRepoMock.Verify(repo => repo.UpdateAsync(It.IsAny<Asset>()), Times.Never);
            _assignmentRepoMock.Verify(repo => repo.UpdateAsync(It.IsAny<Assignment>()), Times.Once);
            Assert.That(response, Is.Null);
        }

        [Test]
        public void UpdateAsync_ChangeUserOnly_UserNotFound_ThrowsNotFoundException()
        {
            // Arrange
            var dto = new AssignmentDTO { AssetId = 1, AssignedToId = 2 }; // Changing user only
            int assignmentId = 1;
            string modifiedName = "testUser";
            var currentAssignment = new Assignment
            {
                AssetId = 1,
                AssignedToId = 1,
                State = AssignmentState.WaitingForAcceptance,
                AssignedTo = new User(),
                Asset = new Asset { Id = 1, State = AssetState.Assigned }
            };

            _assignmentRepoMock.Setup(repo => repo.FindAssignmentByIdWithoutAsset(assignmentId))
                .ReturnsAsync(currentAssignment);
            _userRepoMock.Setup(repo => repo.GetByIdAsync(dto.AssignedToId))
                .ReturnsAsync((User?)null); // User not found

            // Act & Assert
            var ex = Assert.ThrowsAsync<Exception>(async () => await _assignmentService.UpdateAsync(dto, assignmentId, modifiedName));

            // Check that the inner exception is NotFoundException
            Assert.That(ex.InnerException, Is.InstanceOf<NotFoundException>());
            Assert.That(ex.InnerException.Message, Contains.Substring("Not found user"));
        }

        [Test]
        public void UpdateAsync_NewAssetAlreadyAssigned_ThrowsDataInvalidException()
        {
            // Arrange
            var dto = new AssignmentDTO { AssetId = 2, AssignedToId = 1 };
            int assignmentId = 1;
            string modifiedName = "testUser";

            var assignment = new Assignment { AssetId = 1, AssignedToId = 1, State = AssignmentState.WaitingForAcceptance };
            var newAsset = new Asset { State = AssetState.Assigned };

            _assignmentRepoMock.Setup(repo => repo.FindAssignmentByIdWithoutAsset(assignmentId))
                .ReturnsAsync(assignment);
            _assetRepoMock.Setup(repo => repo.GetByIdAsync(dto.AssetId))
                .ReturnsAsync(newAsset);

            // Act & Assert
            var ex = Assert.ThrowsAsync<Exception>(async () => await _assignmentService.UpdateAsync(dto, assignmentId, modifiedName));

            // Check that the inner exception is DataInvalidException
            Assert.That(ex.InnerException, Is.InstanceOf<DataInvalidException>());
            Assert.That(ex.InnerException.Message, Contains.Substring("Asset has been assigned to other Staff"));
        }

        [Test]
        public void UpdateAsync_AssignmentAccepted_ThrowsDataInvalidException()
        {
            // Arrange
            var dto = new AssignmentDTO { AssetId = 1, AssignedToId = 1 };
            int assignmentId = 1;
            string modifiedName = "testUser";
            var assignment = new Assignment { AssetId = 1, AssignedToId = 1, State = AssignmentState.Accepted };

            _assignmentRepoMock.Setup(repo => repo.FindAssignmentByIdWithoutAsset(assignmentId))
                .ReturnsAsync(assignment);

            // Act & Assert
            var ex = Assert.ThrowsAsync<Exception>(async () => await _assignmentService.UpdateAsync(dto, assignmentId, modifiedName));

            // Check that the inner exception is DataInvalidException
            Assert.That(ex.InnerException, Is.InstanceOf<DataInvalidException>());
            Assert.That(ex.InnerException.Message, Contains.Substring("Assignment is assigned to user"));
        }

        [Test]
        public Task UpdateAsync_ChangeAssetOnly_OldAssetNotFound_ThrowsNotFoundException()
        {
            // Arrange
            var dto = new AssignmentDTO { AssignedToId = 1, AssetId = 2 };
            var assignment = new Assignment { Id = 1, AssignedToId = 1, AssetId = 3, State = AssignmentState.WaitingForAcceptance };

            _assignmentRepoMock.Setup(repo => repo.FindAssignmentByIdWithoutAsset(1))
                .ReturnsAsync(assignment);
            _assetRepoMock.Setup(repo => repo.GetByIdAsync(3))
                .ReturnsAsync((Asset?)null);

            // Act & Assert
            var ex = Assert.ThrowsAsync<Exception>(async () =>
                await _assignmentService.UpdateAsync(dto, 1, "modifier"));
            Assert.IsInstanceOf<NotFoundException>(ex.InnerException);
            Assert.That(ex.InnerException.Message, Is.EqualTo("Not found asset"));
            return Task.CompletedTask;
        }

        [Test]
        public Task UpdateAsync_ChangeUserOnly_OldUserNotFound_ThrowsNotFoundException()
        {
            // Arrange
            var dto = new AssignmentDTO { AssignedToId = 2, AssetId = 1 };
            var assignment = new Assignment { Id = 1, AssignedToId = 1, AssetId = 1, State = AssignmentState.WaitingForAcceptance };

            _assignmentRepoMock.Setup(repo => repo.FindAssignmentByIdWithoutAsset(1))
                .ReturnsAsync(assignment);
            _assetRepoMock.Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync((Asset?)null);

            // Act & Assert
            var ex = Assert.ThrowsAsync<Exception>(async () =>
                await _assignmentService.UpdateAsync(dto, 1, "modifier"));
            Assert.IsInstanceOf<NotFoundException>(ex.InnerException);
            Assert.That(ex.InnerException.Message, Is.EqualTo("Not found user"));
            return Task.CompletedTask;
        }

        [Test]
        public void UpdateAsync_ChangeAssetAndUser_NewAssetNotFound_ThrowsNotFoundException()
        {
            // Arrange
            var dto = new AssignmentDTO { AssignedToId = 2, AssetId = 3 };
            var assignment = new Assignment { Id = 1, AssignedToId = 1, AssetId = 1, State = AssignmentState.WaitingForAcceptance };

            _assignmentRepoMock.Setup(repo => repo.FindAssignmentByIdWithoutAsset(1))
                .ReturnsAsync(assignment);
            _assetRepoMock.Setup(repo => repo.GetByIdAsync(3))
                .ReturnsAsync((Asset?)null);

            // Act & Assert
            var ex = Assert.ThrowsAsync<Exception>(async () =>
                await _assignmentService.UpdateAsync(dto, 1, "modifier"));
            Assert.IsInstanceOf<NotFoundException>(ex.InnerException);
            Assert.That(ex.InnerException.Message, Is.EqualTo("Not found asset"));
        }

        [Test]
        public void UpdateAsync_ChangeAssetAndUser_NewUserNotFound_ThrowsNotFoundException()
        {
            // Arrange
            var dto = new AssignmentDTO { AssignedToId = 2, AssetId = 3 };
            var assignment = new Assignment { Id = 1, AssignedToId = 1, AssetId = 1, State = AssignmentState.WaitingForAcceptance };

            _assignmentRepoMock.Setup(repo => repo.FindAssignmentByIdWithoutAsset(1))
                .ReturnsAsync(assignment);
            _assetRepoMock.Setup(repo => repo.GetByIdAsync(3))
                .ReturnsAsync(new Asset { Id = 3, State = AssetState.Available });
            _userRepoMock.Setup(repo => repo.GetByIdAsync(2))
                .ReturnsAsync((User?)null);

            // Act & Assert
            var ex = Assert.ThrowsAsync<Exception>(async () =>
                await _assignmentService.UpdateAsync(dto, 1, "modifier"));
            Assert.IsInstanceOf<NotFoundException>(ex.InnerException);
            Assert.That(ex.InnerException.Message, Is.EqualTo("Not found asset"));
        }

        [Test]
        public void UpdateAsync_ChangeUserOnly_NewUserNotFound_ThrowsNotFoundException()
        {
            // Arrange
            var dto = new AssignmentDTO { AssignedToId = 2, AssetId = 1 };
            var assignment = new Assignment
            { Id = 1, AssignedToId = 1, AssetId = 1, State = AssignmentState.WaitingForAcceptance };

            _assignmentRepoMock.Setup(repo => repo.FindAssignmentByIdWithoutAsset(1))
                .ReturnsAsync(assignment);
            _userRepoMock.Setup(repo => repo.GetByIdAsync(2))
                .ReturnsAsync((User?)null);

            // Act & Assert
            var ex = Assert.ThrowsAsync<Exception>(async () =>
                await _assignmentService.UpdateAsync(dto, 1, "modifier"));
            Assert.IsInstanceOf<NotFoundException>(ex.InnerException);
            Assert.That(ex.InnerException.Message, Is.EqualTo("Not found user"));
        }

        [Test]
        public async Task RespondAssignment_WhenAssignmentAccepted_UpdatesAssignment()
        {
            // Arrange
            var assignmentId = 1;
            var assignment = new Assignment { Id = assignmentId, AssetId = 2, State = AssignmentState.WaitingForAcceptance, AssignedToId = 1 }; // Set the State to WaitingForAcceptance
            var dto = new AssignmentRespondDto { State = AssignmentState.Accepted, AssignedToID = 1, AssetID = 2 };

            _assignmentRepoMock.Setup(repo => repo.FindAssignmentByIdWithoutAsset(assignmentId))
                .ReturnsAsync(assignment);

            // Act
            await _assignmentService.RespondAssignment(dto, assignmentId);

            // Assert
            _mapperMock.Verify(mapper => mapper.Map(dto, assignment), Times.Once);
            _assignmentRepoMock.Verify(repo => repo.UpdateAsync(assignment), Times.Once);
            _assetRepoMock.Verify(repo => repo.GetByIdAsync(It.IsAny<int>()), Times.Never);
        }

        [Test]
        public async Task RespondAssignment_WhenAssignmentDeclined_UpdatesAssetAndDeletesAssignment()
        {
            // Arrange
            var assignmentId = 1;
            var assetId = 2;
            var assignment = new Assignment { Id = assignmentId, AssetId = assetId, State = AssignmentState.WaitingForAcceptance, AssignedToId = 1 }; // Set the State to WaitingForAcceptance
            var asset = new Asset { Id = assetId, Assignments = new List<Assignment> { assignment }, State = AssetState.Assigned };
            var dto = new AssignmentRespondDto { State = AssignmentState.Declined, AssignedToID = 1, AssetID = 2 }; // Set the State to Declined

            _assignmentRepoMock.Setup(repo => repo.FindAssignmentByIdWithoutAsset(assignmentId))
                .ReturnsAsync(assignment);
            _assetRepoMock.Setup(repo => repo.GetByIdAsync(assetId))
                .ReturnsAsync(asset);

            // Act
            await _assignmentService.RespondAssignment(dto, assignmentId);

            // Assert
            Assert.That(asset.Assignments, Is.Null);
            Assert.That(asset.State, Is.EqualTo(AssetState.Available));
            _assetRepoMock.Verify(repo => repo.UpdateAsync(asset), Times.Once);
            _assignmentRepoMock.Verify(repo => repo.UpdateAsync(assignment), Times.Once);
        }
        [Test]
        public async Task DeleteAsync_ShouldThrowDataInvalidException_WhenAssignmentStateIsNotValid()
        {
            // Arrange
            var id = 1;
            var assignment = new Assignment { Id = id, State = AssignmentState.Accepted };

            _assignmentRepoMock.Setup(x => x.GetByIdAsync(id))
                                     .ReturnsAsync(assignment);

            // Act & Assert
            Assert.ThrowsAsync<DataInvalidException>(async () => await _assignmentService.DeleteAsync(id));
        }

        [Test]
        public async Task DeleteAsync_ShouldDeleteAssignmentAndUpdateAssetState()
        {
            // Arrange
            var id = 1;
            var assignment = new Assignment { Id = id, State = AssignmentState.WaitingForAcceptance, Asset = new Asset { Id = 1, State = AssetState.Assigned } };
            var asset = new Asset { Id = 1, State = AssetState.Assigned };

            _assignmentRepoMock.Setup(x => x.GetByIdAsync(id))
                                     .ReturnsAsync(assignment);
            _assignmentRepoMock.Setup(x => x.DeleteAsync(assignment))
                                     .Returns(Task.CompletedTask);
            _assetRepoMock.Setup(x => x.UpdateAsync(It.IsAny<Asset>()))
                                .Returns(Task.CompletedTask);
            _mapperMock.Setup(x => x.Map<Asset>(assignment.Asset))
                       .Returns(asset);

            // Act
            await _assignmentService.DeleteAsync(id);

            // Assert
            _assignmentRepoMock.Verify(x => x.DeleteAsync(It.IsAny<Assignment>()), Times.Once);
            _assetRepoMock.Verify(x => x.UpdateAsync(It.Is<Asset>(a => a.State == AssetState.Available)), Times.Once);
        }
        [Test]
        public void RespondAssignment_WhenAssignmentNotFound_ThrowsNotFoundException()
        {
            // Arrange
            var assignmentId = 1;
            var dto = new AssignmentRespondDto { State = AssignmentState.WaitingForAcceptance };

            _assignmentRepoMock.Setup(repo => repo.FindAssignmentByIdWithoutAsset(assignmentId))
                .ReturnsAsync((Assignment?)null);

            // Act & Assert
            Assert.ThrowsAsync<NotFoundException>(() => _assignmentService.RespondAssignment(dto, assignmentId));
        }

        [Test]
        public void RespondAssignment_WhenAssetNotFound_ThrowsDataInvalidException()
        {
            // Arrange
            var assignmentId = 1;
            var assetId = 2;
            var assignment = new Assignment { Id = assignmentId, AssetId = assetId, State = AssignmentState.WaitingForAcceptance }; // Set the State to WaitingForAcceptance
            var dto = new AssignmentRespondDto { State = AssignmentState.WaitingForAcceptance };

            _assignmentRepoMock.Setup(repo => repo.FindAssignmentByIdWithoutAsset(assignmentId))
                .ReturnsAsync(assignment);
            _assetRepoMock.Setup(repo => repo.GetByIdAsync(assetId))
                .ReturnsAsync((Asset?)null);

            // Act & Assert
            Assert.ThrowsAsync<DataInvalidException>(() => _assignmentService.RespondAssignment(dto, assignmentId));
        }

    }
}