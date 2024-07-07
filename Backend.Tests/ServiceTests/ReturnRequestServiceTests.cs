using AutoMapper;
using Backend.Application.Common.Paging;
using Backend.Application.DTOs.ReturnRequestDTOs;
using Backend.Application.IRepositories;
using Backend.Application.Services.ReturnRequestServices;
using Backend.Domain.Entities;
using Backend.Domain.Enum;
using Backend.Domain.Exceptions;
using Moq;

namespace Backend.Tests.ServiceTests;

[TestFixture]
public class ReturnRequestServiceTests
{
    private Mock<IReturnRequestRepository> _requestRepositoryMock;
    private Mock<IAssignmentRepository> _assignmentRepositoryMock;
    private Mock<IMapper> _mapperMock;
    private Mock<IAssetRepository> _assetRepositoryMock;
    private ReturnRequestService _service;

    [SetUp]
    public void SetUp()
    {
        _requestRepositoryMock = new Mock<IReturnRequestRepository>();
        _assignmentRepositoryMock = new Mock<IAssignmentRepository>();
        _mapperMock = new Mock<IMapper>();
        _assetRepositoryMock = new Mock<IAssetRepository>();
        _service = new ReturnRequestService(_requestRepositoryMock.Object, _assignmentRepositoryMock.Object, _mapperMock.Object, _assetRepositoryMock.Object);
    }

    [Test]
    public async Task GetByIdAsync_WhenRequestExists_ReturnsCorrectResponse()
    {
        // Arrange
        var requestId = 1;
        var request = new ReturnRequest { Id = requestId };
        var response = new ReturnRequestResponse { Id = requestId };

        _requestRepositoryMock.Setup(repo => repo.GetByIdAsync(requestId)).ReturnsAsync(request);
        _mapperMock.Setup(mapper => mapper.Map<ReturnRequestResponse>(request)).Returns(response);

        // Act
        var result = await _service.GetByIdAsync(requestId);

        // Assert
        Assert.That(result, Is.EqualTo(response));
    }
    [Test]
    public async Task CreateRequest_WhenValidInputs_ProceedsAsExpected()
    {
        // Arrange
        var assignmentId = 1;
        var createName = "Test User";
        var createId = 1;
        var role = Role.Admin;
        var assignment = new Assignment { Id = assignmentId, AssignedToId = createId, State = AssignmentState.Accepted, IsDeleted = false, ReturnRequest = null };

        _assignmentRepositoryMock.Setup(repo => repo.GetByIdAsync(assignmentId)).ReturnsAsync(assignment);
        _requestRepositoryMock.Setup(repo => repo.InsertAsync(It.IsAny<ReturnRequest>())).Returns(Task.CompletedTask);
        _assignmentRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<Assignment>())).Returns(Task.CompletedTask);

        // Act
        await _service.CreateRequest(assignmentId, createName, createId, role);

        // Assert
        _requestRepositoryMock.Verify(repo => repo.InsertAsync(It.IsAny<ReturnRequest>()), Times.Once);
        _assignmentRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<Assignment>()), Times.Once);
    }

    [Test]
    public void CreateRequest_WhenUserIsStaffAndNotAssignedToAsset_ThrowsForbiddenException()
    {
        // Arrange
        var assignmentId = 1;
        var createName = "Test User";
        var createId = 2; // Different from AssignedToId
        var role = Role.Staff;
        var assignment = new Assignment { Id = assignmentId, AssignedToId = 1, State = AssignmentState.Accepted, IsDeleted = false, ReturnRequest = null };

        _assignmentRepositoryMock.Setup(repo => repo.GetByIdAsync(assignmentId)).ReturnsAsync(assignment);

        // Act & Assert
        Assert.ThrowsAsync<ForbiddenException>(() => _service.CreateRequest(assignmentId, createName, createId, role));
    }

    [Test]
    public void CreateRequest_WhenAssignmentNotAccepted_ThrowsDataInvalidException()
    {
        // Arrange
        var assignmentId = 1;
        var createName = "Test User";
        var createId = 1;
        var role = Role.Admin;
        var assignment = new Assignment { Id = assignmentId, AssignedToId = createId, State = AssignmentState.WaitingForAcceptance, IsDeleted = false, ReturnRequest = null };

        _assignmentRepositoryMock.Setup(repo => repo.GetByIdAsync(assignmentId)).ReturnsAsync(assignment);

        // Act & Assert
        Assert.ThrowsAsync<DataInvalidException>(() => _service.CreateRequest(assignmentId, createName, createId, role));
    }

    [Test]
    public void CreateRequest_WhenAssetHasBeenReturned_ThrowsDataInvalidException()
    {
        // Arrange
        var assignmentId = 1;
        var createName = "Test User";
        var createId = 1;
        var role = Role.Admin;
        var assignment = new Assignment { Id = assignmentId, AssignedToId = createId, State = AssignmentState.Accepted, IsDeleted = true, ReturnRequest = null };

        _assignmentRepositoryMock.Setup(repo => repo.GetByIdAsync(assignmentId)).ReturnsAsync(assignment);

        // Act & Assert
        Assert.ThrowsAsync<DataInvalidException>(() => _service.CreateRequest(assignmentId, createName, createId, role));
    }

    [Test]
    public void CreateRequest_WhenAssignmentAlreadyHasReturnRequest_ThrowsDataInvalidException()
    {
        // Arrange
        var assignmentId = 1;
        var createName = "Test User";
        var createId = 1;
        var role = Role.Admin;
        var assignment = new Assignment { Id = assignmentId, AssignedToId = createId, State = AssignmentState.Accepted, IsDeleted = false, ReturnRequest = new ReturnRequest() };

        _assignmentRepositoryMock.Setup(repo => repo.GetByIdAsync(assignmentId)).ReturnsAsync(assignment);

        // Act & Assert
        Assert.ThrowsAsync<DataInvalidException>(() => _service.CreateRequest(assignmentId, createName, createId, role));
    }

    [Test]
    public async Task GetFilterAsync_WhenValidInputs_ReturnsCorrectResponse()
    {
        // Arrange
        var request = new ReturnRequestFilterRequest();
        var location = Location.HaNoi;
        var returnRequests = new List<ReturnRequest> { new ReturnRequest() };
        var paginationResponse = new PaginationResponse<ReturnRequest>(returnRequests, 1);
        var returnRequestResponses = new List<ReturnRequestResponse> { new ReturnRequestResponse() };

        _requestRepositoryMock.Setup(repo => repo.GetFilterAsync(request, location)).ReturnsAsync(paginationResponse);
        _mapperMock.Setup(mapper => mapper.Map<IEnumerable<ReturnRequestResponse>>(returnRequests)).Returns(returnRequestResponses);

        // Act
        var result = await _service.GetFilterAsync(request, location);

        // Assert
        Assert.That(result.Data, Is.EqualTo(returnRequestResponses));
        Assert.That(result.TotalCount, Is.EqualTo(1));
    }

    [Test]
    public async Task CancelRequestAsync_WhenValidInputs_ProceedsAsExpected()
    {
        // Arrange
        var id = 1;
        var modifyName = "Test User";
        var role = Role.Admin;
        var request = new ReturnRequest { Id = id, State = ReturnRequestState.WaitingForReturning };
        var assignment = new Assignment { Id = id, State = AssignmentState.Accepted };

        _requestRepositoryMock.Setup(repo => repo.GetByIdAsync(id)).ReturnsAsync(request);
        _assignmentRepositoryMock.Setup(repo => repo.FindAssignmentByIdWithoutAsset(request.AssignmentId)).ReturnsAsync(assignment);
        _requestRepositoryMock.Setup(repo => repo.DeleteAsync(request)).Returns(Task.CompletedTask);
        _assignmentRepositoryMock.Setup(repo => repo.UpdateAsync(assignment)).Returns(Task.CompletedTask);

        // Act
        await _service.CancelRequestAsync(id, modifyName, role);

        // Assert
        _requestRepositoryMock.Verify(repo => repo.DeleteAsync(request), Times.Once);
        _assignmentRepositoryMock.Verify(repo => repo.UpdateAsync(assignment), Times.Once);
    }
    
    [Test]
    public void CancelRequestAsync_WhenUserIsStaff_ThrowsForbiddenException()
    {
        // Arrange
        var id = 1;
        var modifyName = "Test User";
        var role = Role.Staff;
        var request = new ReturnRequest { Id = id, State = ReturnRequestState.WaitingForReturning };

        _requestRepositoryMock.Setup(repo => repo.GetByIdAsync(id)).ReturnsAsync(request);

        // Act & Assert
        Assert.ThrowsAsync<ForbiddenException>(() => _service.CancelRequestAsync(id, modifyName, role));
    }

    [Test]
    public void CancelRequestAsync_WhenRequestStateIsCompleted_ThrowsDataInvalidException()
    {
        // Arrange
        var id = 1;
        var modifyName = "Test User";
        var role = Role.Admin;
        var request = new ReturnRequest { Id = id, State = ReturnRequestState.Completed };

        _requestRepositoryMock.Setup(repo => repo.GetByIdAsync(id)).ReturnsAsync(request);

        // Act & Assert
        Assert.ThrowsAsync<DataInvalidException>(() => _service.CancelRequestAsync(id, modifyName, role));
    }
    
    [Test]
    public async Task CompleteRequestAsync_WhenValidInputs_ProceedsAsExpected()
    {
        // Arrange
        var id = 1;
        var acceptedBy = 1;
        var asset = new Asset { State = AssetState.Assigned };
        var assignment = new Assignment { Id = id, State = AssignmentState.WaitingForReturning, Asset = asset };
        var request = new ReturnRequest { Id = id, State = ReturnRequestState.WaitingForReturning, AssignmentId = id, Assignment = assignment };

        _requestRepositoryMock.Setup(repo => repo.GetByIdAsync(id)).ReturnsAsync(request);
        _assignmentRepositoryMock.Setup(repo => repo.FindAssignmentByIdWithoutAsset(request.AssignmentId)).ReturnsAsync(assignment);
        _requestRepositoryMock.Setup(repo => repo.UpdateAsync(request)).Returns(Task.CompletedTask);
        _assignmentRepositoryMock.Setup(repo => repo.UpdateAsync(assignment)).Returns(Task.CompletedTask);

        // Act
        await _service.CompleteRequestAsync(id, acceptedBy);

        // Assert
        _requestRepositoryMock.Verify(repo => repo.UpdateAsync(request), Times.Once);
        _assignmentRepositoryMock.Verify(repo => repo.UpdateAsync(assignment), Times.Once);
    }

    [Test]
    public void CompleteRequestAsync_WhenRequestNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var id = 1;
        var acceptedBy = 1;

        _requestRepositoryMock.Setup(repo => repo.GetByIdAsync(id)).ReturnsAsync((ReturnRequest)null);

        // Act & Assert
        Assert.ThrowsAsync<NotFoundException>(() => _service.CompleteRequestAsync(id, acceptedBy));
    }

    [Test]
    public void CompleteRequestAsync_WhenAssignmentNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var id = 1;
        var acceptedBy = 1;
        var request = new ReturnRequest { Id = id, State = ReturnRequestState.WaitingForReturning, AssignmentId = id };

        _requestRepositoryMock.Setup(repo => repo.GetByIdAsync(id)).ReturnsAsync(request);
        _assignmentRepositoryMock.Setup(repo => repo.FindAssignmentByIdWithoutAsset(request.AssignmentId)).ReturnsAsync((Assignment)null);

        // Act & Assert
        Assert.ThrowsAsync<NotFoundException>(() => _service.CompleteRequestAsync(id, acceptedBy));
    }
}