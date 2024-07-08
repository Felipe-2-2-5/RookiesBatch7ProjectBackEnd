using NUnit.Framework;
using Moq;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Backend.API.Controllers;
using Backend.Application.Common.Paging;
using Backend.Application.DTOs.AssignmentDTOs;
using Backend.Application.Services.AssignmentServices;
using Backend.Domain.Enum;
using Backend.Application.DTOs.AuthDTOs;
using Backend.Application.DTOs.AssetDTOs;
using Microsoft.AspNetCore.Http;

namespace Backend.Tests.ControllerTests
{
    [TestFixture]
    public class AssignmentControllerTests
    {
        private Mock<IAssignmentService> _mockAssignmentService;
        private AssignmentController _controller;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            _mockAssignmentService = new Mock<IAssignmentService>();
            _controller = new AssignmentController(_mockAssignmentService.Object);

            // Mock the User.Claims for the AssignedById property
            var claims = new List<Claim>
            {
                new("UserName", "testuser"),
                new("Location", Location.HaNoi.ToString()),
                new("UserId", "123"),
                new(ClaimTypes.Role, Role.Admin.ToString())
            };
            var identity = new ClaimsIdentity(claims, "TestAuth");
            var user = new ClaimsPrincipal(identity);

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };
        }

        [Test]
        public async Task GetByIdAsync_ValidId_ReturnsOk()
        {
            // Arrange
            int assignmentId = 1;
            var expectedDto = new AssignmentResponse
            {
                Id = assignmentId,
                AssignedToId = 456,
                AssignedDate = DateTime.UtcNow,
                AssetId = 789,
                Note = "Test assignment",
                State = AssignmentState.Accepted,
                AssignedTo = new UserResponse { Id = 456, UserName = "user456" },
                AssignedBy = new UserResponse { Id = 789, UserName = "admin789" },
                Asset = new AssetResponse { Id = 789, AssetName = "Asset ABC" },
                ReturnRequest = null
            };
            _mockAssignmentService.Setup(x => x.GetByIdAsync(assignmentId)).ReturnsAsync(expectedDto);

            // Act
            var result = await _controller.GetByIdAsync(assignmentId);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult.Value, Is.EqualTo(expectedDto));
        }

        [Test]
        public async Task GetFilterAsync_ValidRequest_ReturnsOk()
        {
            // Arrange
            var request = new AssignmentFilterRequest
            {
                State = "Accepted",
                AssignedDate = DateTime.UtcNow.Date
            };
            var expectedData = new List<AssignmentResponse>
            {
                new() { Id = 1, AssignedToId = 456, AssignedDate = DateTime.UtcNow, AssetId = 789, Note = "Assignment 1" },
                new() { Id = 2, AssignedToId = 457, AssignedDate = DateTime.UtcNow.AddDays(-1), AssetId = 790, Note = "Assignment 2" }
            };
            var paginationResponse = new PaginationResponse<AssignmentResponse>(expectedData, expectedData.Count);
            _mockAssignmentService.Setup(x => x.GetFilterAsync(request, It.IsAny<Location>())).ReturnsAsync(paginationResponse);

            // Act
            var result = await _controller.GetFilterAsync(request);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult.Value, Is.InstanceOf<PaginationResponse<AssignmentResponse>>());

            var returnedData = (PaginationResponse<AssignmentResponse>)okResult.Value;
            Assert.Multiple(() =>
            {
                Assert.That(returnedData.TotalCount, Is.EqualTo(expectedData.Count));
                Assert.That(returnedData.Data, Is.EqualTo(expectedData));
            });
        }

        [Test]
        public async Task InsertAsync_ValidDto_ReturnsOk()
        {
            // Arrange
            var dto = new AssignmentDTO
            {
                AssignedToId = 456,
                AssignedDate = DateTime.UtcNow,
                AssetId = 789,
                Note = "Test assignment"
            };
            var expectedRes = new AssignmentResponse
            {
                Id = 1,
                AssignedToId = dto.AssignedToId,
                AssignedDate = dto.AssignedDate.Value,
                AssetId = dto.AssetId,
                Note = dto.Note,
                State = AssignmentState.WaitingForAcceptance,
                AssignedTo = new UserResponse { Id = 456, UserName = "user456" },
                AssignedBy = new UserResponse { Id = 789, UserName = "admin789" },
                Asset = new AssetResponse { Id = 789, AssetName = "Asset ABC" },
                ReturnRequest = null
            };
            _mockAssignmentService.Setup(x => x.InsertAsync(dto, It.IsAny<string>(), It.IsAny<int>())).ReturnsAsync(expectedRes);

            // Act
            var result = await _controller.InsertAsync(dto);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult.Value, Is.EqualTo(expectedRes));
        }

        [Test]
        public async Task UpdateAsync_ValidDtoAndId_ReturnsOk()
        {
            // Arrange
            int assignmentId = 1;
            var dto = new AssignmentDTO
            {
                AssignedToId = 456,
                AssignedDate = DateTime.UtcNow,
                AssetId = 789,
                Note = "Updated assignment"
            };
            var expectedRes = new AssignmentResponse
            {
                Id = assignmentId,
                AssignedToId = dto.AssignedToId,
                AssignedDate = dto.AssignedDate.Value,
                AssetId = dto.AssetId,
                Note = dto.Note,
                State = AssignmentState.WaitingForAcceptance,
                AssignedTo = new UserResponse { Id = 456, UserName = "user456" },
                AssignedBy = new UserResponse { Id = 789, UserName = "admin789" },
                Asset = new AssetResponse { Id = 789, AssetName = "Asset ABC" },
                ReturnRequest = null
            };
            _mockAssignmentService.Setup(x => x.UpdateAsync(dto, assignmentId, It.IsAny<string>())).ReturnsAsync(expectedRes);

            // Act
            var result = await _controller.UpdateAsync(dto, assignmentId);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult.Value, Is.EqualTo(expectedRes));
        }

        [Test]
        public async Task DeleteAsync_ValidId_ReturnsNoContent()
        {
            // Arrange
            int assignmentId = 1;
            _mockAssignmentService.Setup(x => x.DeleteAsync(assignmentId)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteAsync(assignmentId);

            // Assert
            Assert.That(result, Is.InstanceOf<NoContentResult>());
        }

        [Test]
        public async Task GetMyAssignmentsAsync_ValidRequest_ReturnsOk()
        {
            // Arrange
            var request = new MyAssignmentFilterRequest
            {
                UserId = 123 // Example user ID
            };
            var expectedData = new List<AssignmentResponse>
            {
                new() { Id = 1, AssignedToId = 123, AssignedDate = DateTime.UtcNow, AssetId = 456, Note = "Assignment 1" },
                new() { Id = 2, AssignedToId = 123, AssignedDate = DateTime.UtcNow.AddDays(-1), AssetId = 789, Note = "Assignment 2" }
            };
            var paginationResponse = new PaginationResponse<AssignmentResponse>(expectedData, expectedData.Count);
            _mockAssignmentService.Setup(x => x.GetMyAssignmentsAsync(request)).ReturnsAsync(paginationResponse);

            // Act
            var result = await _controller.GetMyAssignmentsAsync(request);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult.Value, Is.InstanceOf<PaginationResponse<AssignmentResponse>>());

            var returnedData = (PaginationResponse<AssignmentResponse>)okResult.Value;
            Assert.Multiple(() =>
            {
                Assert.That(returnedData.TotalCount, Is.EqualTo(expectedData.Count));
                Assert.That(returnedData.Data, Is.EqualTo(expectedData));
            });
        }

        [Test]
        public async Task AssignmentRespond_ValidDtoAndId_ReturnsOk()
        {
            // Arrange
            int assignmentId = 1;
            var dto = new AssignmentRespondDto
            {
                State = AssignmentState.Accepted
            };
            _mockAssignmentService.Setup(x => x.RespondAssignment(dto, assignmentId)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.AssignmentRespond(dto, assignmentId);

            // Assert
            Assert.That(result, Is.InstanceOf<OkResult>());
        }

    }
}
