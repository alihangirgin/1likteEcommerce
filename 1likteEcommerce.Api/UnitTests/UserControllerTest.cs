using _1likteEcommerce.Api.Controllers;
using _1likteEcommerce.Api.Models;
using _1likteEcommerce.Core.Constants;
using _1likteEcommerce.Core.Models;
using _1likteEcommerce.Core.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace _1likteEcommerce.Api.UnitTests
{
    public class UserControllerTests
    {
        private readonly Mock<UserManager<User>> _userManagerMock;
        private readonly Mock<SignInManager<User>> _signInManagerMock;
        private readonly Mock<IUserService> _userServiceMock;
        private readonly Mock<IFileService> _fileServiceMock;
        private readonly UserController _controller;

        public UserControllerTests()
        {
            _userManagerMock = new Mock<UserManager<User>>(
                Mock.Of<IUserStore<User>>(),
                null, null, null, null, null, null, null, null
            );
            _signInManagerMock = new Mock<SignInManager<User>>(_userManagerMock.Object, Mock.Of<IHttpContextAccessor>(), Mock.Of<IUserClaimsPrincipalFactory<User>>(), null, null, null, null);
            _userServiceMock = new Mock<IUserService>();
            _fileServiceMock = new Mock<IFileService>();

            _controller = new UserController(_signInManagerMock.Object, _userManagerMock.Object, _userServiceMock.Object, _fileServiceMock.Object);
        }

        [Fact]
        public async Task Register_ReturnsOk()
        {
            // Arrange
            var model = new RegisterModel
            {
                Username = "testuserNew",
                Email = "testuserNew@example.com",
                Password = "Test.123"
            };

            var user = new User { UserName = model.Username, Email = model.Email };
            var identityResult = IdentityResult.Success;
            _userManagerMock.Setup(x => x.CreateAsync(It.IsAny<User>(), model.Password)).ReturnsAsync(identityResult);
            _signInManagerMock.Setup(x => x.SignInAsync(It.IsAny<User>(), false, It.IsAny<string>())).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Register(model);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("User Created", okResult.Value);
        }


        [Fact]
        public async Task Login_ReturnsOk()
        {
            // Arrange
            var model = new LoginModel
            {
                Username =  TestUserConstants.UnitTestUserName,
                Password = TestUserConstants.UnitTestPassword
            };

            var signInResult = Microsoft.AspNetCore.Identity.SignInResult.Success;
            _signInManagerMock.Setup(x => x.PasswordSignInAsync(model.Username, model.Password, false, true))
                              .ReturnsAsync(signInResult);
            _userServiceMock.Setup(x => x.GenerateJwtToken(It.IsAny<User>())).Returns("mockJwtToken");

            var user = new User { UserName = model.Username };
            _userManagerMock.Setup(x => x.FindByNameAsync(model.Username)).ReturnsAsync(user);

            // Act
            var result = await _controller.Login(model);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);
        }

 
        [Fact]
        public async Task UpdateUser_ReturnsOk()
        {
            // Arrange
            var model = new UpdateUserModel { Email = "testuserNew@example.com", Username = "testuserNew" };
            var user = new User { Id = TestUserConstants.UnitTestUserId, UserName = TestUserConstants.UnitTestUserName, Email = TestUserConstants.UnitTestEmail };

            _userManagerMock.Setup(x => x.FindByIdAsync(TestUserConstants.UnitTestUserId)).ReturnsAsync(user);
            _userManagerMock.Setup(x => x.UpdateAsync(It.IsAny<User>())).ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _controller.UpdateUser(TestUserConstants.UnitTestUserId, model);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("User updated", okResult.Value);
        }

        [Fact]
        public async Task UpdateUser_ReturnsNotFound()
        {
            // Arrange
            var model = new UpdateUserModel { Email = "updatedemail@example.com", Username = "updatedUsername" };

            _userManagerMock.Setup(x => x.FindByIdAsync("9999999")).ReturnsAsync((User)null);

            // Act
            var result = await _controller.UpdateUser("9999999", model);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task UploadProfilePhoto_ReturnsOk()
        {
            // Arrange
            var userId = TestUserConstants.UnitTestUserId;
            var user = new User { Id = userId };

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "testfile.png");
            var fileBytes = File.ReadAllBytes(uploadsFolder); 
            var memoryStream = new MemoryStream(fileBytes);

            var fileMock = new Mock<IFormFile>();
            fileMock.Setup(f => f.CopyToAsync(It.IsAny<Stream>(), default)).Returns(Task.CompletedTask);
            fileMock.Setup(f => f.Length).Returns(memoryStream.Length);
            fileMock.Setup(f => f.OpenReadStream()).Returns(memoryStream);
            fileMock.Setup(f => f.ContentType).Returns("image/png");
            fileMock.Setup(f => f.FileName).Returns("testfile.png");

            _userManagerMock.Setup(x => x.FindByIdAsync(userId)).ReturnsAsync(user);
            _fileServiceMock.Setup(x => x.UploadFileAsync(It.IsAny<byte[]>(), It.IsAny<string>())).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.UploadProfilePhoto(userId, fileMock.Object);

            // Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task UploadProfilePhoto_Failed()
        {
            // Arrange
            IFormFile file = null;

            // Act
            var result = await _controller.UploadProfilePhoto("1", file);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task DownloadProfilePhoto_Ok()
        {
            // Arrange
            var userId = TestUserConstants.UnitTestUserId;
            var user = new User { Id = userId, PhotoPath = "photo.png" };
            var photoBytes = new byte[] { 1, 2, 3 };
            _userManagerMock.Setup(x => x.FindByIdAsync(userId)).ReturnsAsync(user);
            _fileServiceMock.Setup(x => x.GetFileBytesAsync(user.PhotoPath)).ReturnsAsync(photoBytes);

            // Act
            var result = await _controller.DownloadProfilePhoto(userId);

            // Assert
            var fileResult = Assert.IsType<FileContentResult>(result);
            Assert.Equal(photoBytes, fileResult.FileContents);
        }

        [Fact]
        public async Task DownloadProfilePhoto_NotFound()
        {
            // Arrange
            var userId = "999999";
            _userManagerMock.Setup(x => x.FindByIdAsync(userId)).ReturnsAsync((User)null);

            // Act
            var result = await _controller.DownloadProfilePhoto(userId);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }
    }

}
