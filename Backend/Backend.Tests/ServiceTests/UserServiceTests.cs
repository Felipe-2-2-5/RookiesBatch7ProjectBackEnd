using AutoMapper;
using Backend.Application.AuthProvide;
using Backend.Application.Common.Paging;
using Backend.Application.DTOs.AuthDTOs;
using Backend.Application.IRepositories;
using Backend.Application.Services.UserServices;
using Backend.Domain.Entity;
using Backend.Domain.Exceptions;
using Moq;
using System.Security.Claims;

namespace Backend.Tests.ServiceTests
{
    public class UserServiceTests
    {
        private Mock<IUserRepository> _userRepoMock;
        private Mock<ITokenService> _tokenServiceMock;
        private Mock<IMapper> _mapperMock;
        private UserService _userService;

        [SetUp]
        public void Setup()
        {
            _userRepoMock = new Mock<IUserRepository>();
            _tokenServiceMock = new Mock<ITokenService>();
            _mapperMock = new Mock<IMapper>();
            _userService = new UserService(_userRepoMock.Object, _tokenServiceMock.Object, _mapperMock.Object);
        }

        [Test]
        public async Task GetByIdAsync_UserExists_ReturnsUserResponse()
        {
            // Arrange
            var userId = 1;
            var user = new User { Id = userId, UserName = "testuser" };
            var userResponse = new UserResponse { Id = userId, UserName = "testuser" };

            _userRepoMock.Setup(repo => repo.GetByIdAsync(userId)).ReturnsAsync(user);
            _mapperMock.Setup(mapper => mapper.Map<UserResponse>(user)).Returns(userResponse);

            // Act
            var result = await _userService.GetByIdAsync(userId);

            // Assert
            Assert.AreEqual(userResponse, result);
        }

        [Test]
        public void GetByIdAsync_UserDoesNotExist_ThrowsNotFoundException()
        {
            // Arrange
            var userId = 1;

            _userRepoMock.Setup(repo => repo.GetByIdAsync(userId)).ReturnsAsync((User)null);

            // Act & Assert
            Assert.ThrowsAsync<NotFoundException>(async () => await _userService.GetByIdAsync(userId));
        }

        [Test]
        public async Task InsertAsync_ValidUser_ReturnsUserResponse()
        {
            // Arrange
            var userDTO = new UserDTO { };
            var user = new User { UserName = "newuser" };
            var generatedUser = new User { UserName = "newuser", Id = 1 };
            var userResponse = new UserResponse { UserName = "newuser", Id = 1 };

            _mapperMock.Setup(mapper => mapper.Map<User>(userDTO)).Returns(user);
            _userRepoMock.Setup(repo => repo.GenerateUserInformation(user)).ReturnsAsync(generatedUser);
            _userRepoMock.Setup(repo => repo.InsertAsync(generatedUser)).Returns(Task.CompletedTask);
            _userRepoMock.Setup(repo => repo.FindUserByUserNameAsync(generatedUser.UserName)).ReturnsAsync(generatedUser);
            _mapperMock.Setup(mapper => mapper.Map<UserResponse>(generatedUser)).Returns(userResponse);

            // Act
            var result = await _userService.InsertAsync(userDTO);

            // Assert
            Assert.That(result, Is.EqualTo(userResponse));
        }

        [Test]
        public async Task ChangePasswordAsync_ValidPasswordChange_ReturnsTrue()
        {
            // Arrange
            var changePasswordDTO = new ChangePasswordDTO { Id = 1, OldPassword = "oldpassword", NewPassword = "NewPassword1!" };
            var user = new User { Id = 1, Password = "oldpassword", FirstName = "John" };

            _userRepoMock.Setup(repo => repo.GetByIdAsync(changePasswordDTO.Id)).ReturnsAsync(user);
            _userRepoMock.Setup(repo => repo.UpdateAsync(It.IsAny<User>())).Returns(Task.CompletedTask);

            // Act
            var result = await _userService.ChangePasswordAsync(changePasswordDTO);

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void ChangePasswordAsync_InvalidOldPassword_ThrowsDataInvalidException()
        {
            // Arrange
            var changePasswordDTO = new ChangePasswordDTO { Id = 1, OldPassword = "wrongpassword", NewPassword = "NewPassword1!" };
            var user = new User { Id = 1, Password = "oldpassword" };

            _userRepoMock.Setup(repo => repo.GetByIdAsync(changePasswordDTO.Id)).ReturnsAsync(user);

            // Act & Assert
            var ex = Assert.ThrowsAsync<DataInvalidException>(async () => await _userService.ChangePasswordAsync(changePasswordDTO));
            Assert.That(ex.Message, Is.EqualTo("Wrong password"));
        }

        [Test]
        public async Task LoginAsync_ValidCredentials_ReturnsLoginResponse()
        {
            // Arrange
            var loginDTO = new LoginDTO { UserName = "testuser", Password = "password" };
            var user = new User { UserName = "testuser", Password = "password" };
            var token = "jwtToken";

            _userRepoMock.Setup(repo => repo.FindUserByUserNameAsync(loginDTO.UserName)).ReturnsAsync(user);
            _tokenServiceMock.Setup(service => service.GenerateJWTWithUser(It.IsAny<User>(), It.IsAny<IEnumerable<Claim>?>())).Returns(token);
            // Act
            var result = await _userService.LoginAsync(loginDTO);

            // Assert

            Assert.That(result.Flag, Is.True);
            Assert.That(result.Message, Is.EqualTo("Login success"));
            Assert.That(result.Token, Is.EqualTo(token));
        }

        [Test]
        public async Task LoginAsync_InvalidCredentials_ReturnsLoginResponse()
        {
            // Arrange
            var loginDTO = new LoginDTO { UserName = "testuser", Password = "wrongpassword" };
            var user = new User { UserName = "testuser", Password = "password" };

            _userRepoMock.Setup(repo => repo.FindUserByUserNameAsync(loginDTO.UserName)).ReturnsAsync(user);

            // Act
            var result = await _userService.LoginAsync(loginDTO);

            // Assert
            Assert.That(result.Flag, Is.False);
            Assert.That(result.Message, Is.EqualTo("Invalid credentials"));
        }

        [Test]
        public async Task GetFilterAsync_ReturnsPaginationResponse()
        { // Arrange
            var users = new List<User> { new User { Id = 1, UserName = "user1" } };
            var userResponses = new List<UserResponse> { new UserResponse { Id = 1, UserName = "user1" } };
            var paginationResponse = new PaginationResponse<User>(users, 1);

            _userRepoMock.Setup(repo => repo.GetFilterAsync(It.IsAny<FilterRequest>())).ReturnsAsync(paginationResponse);
            _mapperMock.Setup(mapper => mapper.Map<IEnumerable<UserResponse>>(users)).Returns(userResponses);

            // Act
            var result = await _userService.GetFilterAsync(new FilterRequest());

            // Assert
            Assert.That(result.TotalCount, Is.EqualTo(1));
            Assert.That(result.Data, Is.EqualTo(userResponses));
        }
    }

}
