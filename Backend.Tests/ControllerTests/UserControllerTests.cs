using Backend.API.Controllers;
using Backend.Application.Common.Paging;
using Backend.Application.DTOs.AuthDTOs;
using Backend.Application.Services.UserServices;
using Backend.Domain.Enum;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Routing;
using Moq;
using System.Reflection;
using System.Security.Claims;

namespace Backend.Tests.ControllerTests
{
    [TestFixture]
    public class UsersControllerTests
    {
        private Mock<IUserService> _userServiceMock;
        private UsersController _controller;

        [SetUp]
        public void SetUp()
        {
            _userServiceMock = new Mock<IUserService>();

            // Mock HttpContext and User claims
            var httpContext = new DefaultHttpContext();
            httpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
            {
                new Claim("UserName", "TestUser"),
                new Claim("Location", Location.HaNoi.ToString()), // Replace with appropriate location value
                new Claim("UserId", "1"),
                new Claim(ClaimTypes.Role, Role.Admin.ToString()) // Replace with appropriate role value
            }, "mock"));

            // Setup ControllerActionDescriptor
            var controllerActionDescriptor = new ControllerActionDescriptor
            {
                ControllerTypeInfo = typeof(UsersController).GetTypeInfo(), // Replace with your controller type
                MethodInfo = typeof(UsersController).GetMethod("UpdateAsync"), // Replace with your action method
            };

            var actionContext = new ActionContext(httpContext, new RouteData(), controllerActionDescriptor);

            _controller = new UsersController(_userServiceMock.Object)
            {
                ControllerContext = new ControllerContext(actionContext)
            };
        }

        [Test]
        public async Task GetByIdAsync_ReturnsOkResult_WithUser()
        {
            // Arrange
            var userId = 1;
            var userResponse = new UserResponse { Id = userId, UserName = "testuser" };
            _userServiceMock.Setup(s => s.GetByIdAsync(userId)).ReturnsAsync(userResponse);

            // Act
            var result = await _controller.GetByIdAsync(userId);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var returnValue = okResult.Value as UserResponse;
            Assert.IsNotNull(returnValue);
            Assert.AreEqual(userId, returnValue.Id);
        }

        [Test]
        public async Task LoginAsync_ReturnsOkResult_WithLoginResponse()
        {
            // Arrange
            var loginDto = new LoginDTO { UserName = "testuser", Password = "password" };
            var loginResponse = new LoginResponse(true, "Login success");
            _userServiceMock.Setup(s => s.LoginAsync(loginDto)).ReturnsAsync(loginResponse);

            // Act
            var result = await _controller.LoginAsync(loginDto);

            // Assert
            Assert.IsInstanceOf<ActionResult<LoginResponse>>(result);
            var actionResult = result as ActionResult<LoginResponse>;
            Assert.IsNotNull(actionResult);
            Assert.IsInstanceOf<OkObjectResult>(actionResult.Result);
            var okResult = actionResult.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var returnValue = okResult.Value as LoginResponse;
            Assert.IsNotNull(returnValue);
            Assert.IsTrue(returnValue.Flag);
        }



        [Test]
        public async Task GetFilterAsync_ReturnsOkResult_WithFilteredUsers()
        {
            // Arrange
            var request = new UserFilterRequest { Page = 1, PageSize = 10 };
            var response = new PaginationResponse<UserResponse>(new List<UserResponse>(), 0);
            _userServiceMock.Setup(s => s.GetFilterAsync(request, It.IsAny<Location>())).ReturnsAsync(response);

            // Act
            var result = await _controller.GetFilterAsync(request);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var returnValue = okResult.Value as PaginationResponse<UserResponse>;
            Assert.IsNotNull(returnValue);
            Assert.IsEmpty(returnValue.Data);
        }

        [Test]
        public async Task InsertAsync_ReturnsOkResult_WithInsertedUser()
        {
            // Arrange
            var userDto = new UserDTO { FirstName = "manh", LastName = "phan tien" };
            var userResponse = new UserResponse { Id = 1, UserName = "manhpt" };
            _userServiceMock.Setup(s => s.InsertAsync(userDto, "TestUser")).ReturnsAsync(userResponse);

            // Act
            var result = await _controller.InsertAsync(userDto);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var returnValue = okResult.Value as UserResponse;
            Assert.IsNotNull(returnValue);
            Assert.AreEqual("manhpt", returnValue.UserName);
        }

        [Test]
        public async Task DisableUserAsync_ReturnsOkResult()
        {
            // Arrange
            var userId = 1;
            _userServiceMock.Setup(s => s.DisableUserAsync(userId)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DisableUserAsync(userId);

            // Assert
            Assert.IsInstanceOf<OkResult>(result);
        }

        [Test]
        public async Task UpdateAsync_ReturnsOkResult_WithUpdatedUser()
        {
            // Arrange
            var userId = 1;
            var userDto = new UserDTO { FirstName = "Updated", LastName = "User" };
            var userResponse = new UserResponse { Id = userId, FirstName = "Updated", LastName = "User" };
            _userServiceMock.Setup(s => s.UpdateAsync(userId, userDto, "TestUser", Location.HaNoi)).ReturnsAsync(userResponse);

            // Act
            var result = await _controller.UpdateAsync(userId, userDto);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var returnValue = okResult.Value as UserResponse;
            Assert.IsNotNull(returnValue);
            Assert.AreEqual("Updated", returnValue.FirstName);
            Assert.AreEqual("User", returnValue.LastName);
        }
    }
}
