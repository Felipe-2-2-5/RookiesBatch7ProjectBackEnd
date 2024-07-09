using System.Threading.Tasks;
using Backend.API.Controllers;
using Backend.Application.DTOs.ReturnRequestDTOs;
using Backend.Application.Services.ReturnRequestServices;
using Backend.Application.Common.Paging;
using Backend.Domain.Enum;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Controllers;
using System.Reflection;
using System.Security.Claims;
using Microsoft.AspNetCore.Routing;

namespace Backend.API.Tests.Controllers
{
    [TestFixture]
    public class ReturnRequestsControllerTests
    {
        private Mock<IReturnRequestService> _mockRequestService;
        private ReturnRequestsController _controller;

        [SetUp]
        public void Setup()
        {
            _mockRequestService = new Mock<IReturnRequestService>();

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

            _controller = new ReturnRequestsController(_mockRequestService.Object)
            {
                ControllerContext = new ControllerContext(actionContext)
            };
        }

        [Test]
        public async Task InsertAsync_WithValidAssignmentId_ShouldReturnOkResult()
        {
            // Arrange
            int assignmentId = 1;

            // Act
            var result = await _controller.InsertAsync(assignmentId);

            // Assert
            Assert.IsInstanceOf<OkResult>(result);
            var okResult = result as OkResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(StatusCodes.Status200OK, okResult.StatusCode);

            _mockRequestService.Verify(s => s.CreateRequest(assignmentId, It.IsAny<string>(), It.IsAny<int>(), It.IsAny<Role>()), Times.Once);
        }

        [Test]
        public async Task GetByIdAsync_WithValidId_ShouldReturnOkResult()
        {
            // Arrange
            int id = 1;
            var returnRequestDto = new ReturnRequestResponse();
            _mockRequestService.Setup(s => s.GetByIdAsync(id))
                               .ReturnsAsync(returnRequestDto); 

            // Act
            var result = await _controller.GetByIdAsync(id);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.AreEqual(returnRequestDto, okResult.Value);
        }

        [Test]
        public async Task GetFilterAsync_WithValidRequest_ShouldReturnOkResult()
        {
            // Arrange
            var filterRequest = new ReturnRequestFilterRequest();
            var paginationResponse = new PaginationResponse<ReturnRequestResponse>(new ReturnRequestResponse[] { new ReturnRequestResponse() }, 1);

            _mockRequestService.Setup(s => s.GetFilterAsync(filterRequest, Location.HaNoi)) 
                               .ReturnsAsync(paginationResponse);

            try
            {
                // Act
                var result = await _controller.GetFilterAsync(filterRequest);

                // Assert
                Assert.IsInstanceOf<OkObjectResult>(result);
                var okResult = result as OkObjectResult;
                Assert.IsNotNull(okResult);
                Assert.AreEqual(StatusCodes.Status200OK, okResult.StatusCode);
                Assert.AreEqual(paginationResponse, okResult.Value); 
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString()); 
                throw; 
            }
        }

        [Test]
        public async Task CancelRequestAsync_ReturnsNoContent()
        {
            // Arrange
            int requestId = 1;
            string userName = "admin";
            Role userRole = Role.Admin;

            _mockRequestService.Setup(service => service.CancelRequestAsync(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<Role>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.CancelRequestAsync(requestId);

            // Assert
            Assert.IsInstanceOf<NoContentResult>(result);
        }

        [Test]
        public async Task CompleteRequestAsync_ReturnsOk()
        {
            // Arrange
            int requestId = 1;
            int userId = 1;

            _mockRequestService.Setup(service => service.CompleteRequestAsync(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.CompleteRequestAsync(requestId);

            // Assert
            Assert.IsInstanceOf<OkResult>(result);
        }



    }
}
