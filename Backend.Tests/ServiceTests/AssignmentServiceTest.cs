using AutoMapper;
using Backend.Application.Common.Paging;
using Backend.Application.DTOs.AssignmentDTOs;
using Backend.Application.IRepositories;
using Backend.Application.Services.AssignmentServices;
using Backend.Domain.Entities;
using Backend.Domain.Exceptions;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Backend.Tests.ServiceTests
{
    [TestFixture]
    public class AssignmentServiceTests
    {
        private Mock<IAssignmentRepository> _assignmentRepoMock;
        private Mock<IMapper> _mapperMock;
        private AssignmentService _assignmentService;

        [SetUp]
        public void SetUp()
        {
            _assignmentRepoMock = new Mock<IAssignmentRepository>();
            _mapperMock = new Mock<IMapper>();
            _assignmentService = new AssignmentService(_assignmentRepoMock.Object, _mapperMock.Object);
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
            var assignmentDto = new AssignmentDTO();
            var assignment = new Assignment();
            var assignmentResponse = new AssignmentResponse();

            _mapperMock.Setup(mapper => mapper.Map<Assignment>(assignmentDto)).Returns(assignment);
            _assignmentRepoMock.Setup(repo => repo.InsertAsync(assignment)).Returns(Task.CompletedTask);
            _assignmentRepoMock.Setup(repo => repo.FindAssignmentByAssetIdAsync(assignment.AssetId)).ReturnsAsync(assignment);
            _mapperMock.Setup(mapper => mapper.Map<AssignmentResponse>(assignment)).Returns(assignmentResponse);

            // Act
            var result = await _assignmentService.InsertAsync(assignmentDto, "creator");

            // Assert
            Assert.That(result, Is.EqualTo(assignmentResponse));
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
