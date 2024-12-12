using _1likteEcommerce.Api.Controllers;
using _1likteEcommerce.Core.Constants;
using _1likteEcommerce.Core.Dtos;
using _1likteEcommerce.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace _1likteEcommerce.Api.UnitTests
{
    public class CategoryControllerTests
    {
        private readonly Mock<ICategoryService> _categoryServiceMock;
        private readonly CategoryController _controller;

        public CategoryControllerTests()
        {
            _categoryServiceMock = new Mock<ICategoryService>();
            _controller = new CategoryController(_categoryServiceMock.Object);
        }

        [Fact]
        public async Task CreateCategory_ReturnsOk()
        {
            // Arrange
            var model = new CategoryCreateDto { Title = TestUserConstants.UnitTestTitle, Description = TestUserConstants.UnitTestDescription };
            _categoryServiceMock.Setup(x => x.AddCategoryAsync(model))
                                .ReturnsAsync(true);

            // Act
            var result = await _controller.CreateCategory(model);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("category created", okResult.Value);
        }

        [Fact]
        public async Task CreateCategory_ReturnsFailed()
        {
            // Arrange
            var model = new CategoryCreateDto { Title = TestUserConstants.UnitTestTitle };
            _categoryServiceMock.Setup(x => x.AddCategoryAsync(model))
                                .ReturnsAsync(false);

            // Act
            var result = await _controller.CreateCategory(model);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("category cannot created", badRequestResult.Value);
        }

        [Fact]
        public async Task UpdateCategory_ReturnsOk()
        {
            // Arrange
            var model = new CategoryCreateDto { Title = TestUserConstants.UnitTestTitle, Description = TestUserConstants.UnitTestDescription };
            _categoryServiceMock.Setup(x => x.UpdateCategoryAsync(TestUserConstants.UnitTestCategoryId, model))
                                .ReturnsAsync(true);

            // Act
            var result = await _controller.UpdateCategory(TestUserConstants.UnitTestCategoryId, model);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("category updated", okResult.Value);
        }

        [Fact]
        public async Task UpdateCategory_ReturnsFailed()
        {
            // Arrange
            var model = new CategoryCreateDto { Title = TestUserConstants.UnitTestTitle, Description = TestUserConstants.UnitTestDescription };
            _categoryServiceMock.Setup(x => x.UpdateCategoryAsync(TestUserConstants.UnitTestCategoryId, model))
                                .ReturnsAsync(false);

            // Act
            var result = await _controller.UpdateCategory(999999, model);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("category cannot updated", badRequestResult.Value);
        }

        [Fact]
        public async Task GetCategory_ReturnsOk()
        {
            // Arrange
            var category = new CategoryDto { Id = TestUserConstants.UnitTestCategoryId, Title = TestUserConstants.UnitTestTitle, Description = TestUserConstants.UnitTestDescription };
            _categoryServiceMock.Setup(x => x.GetCategoryAsync(TestUserConstants.UnitTestCategoryId))
                                .ReturnsAsync(category);

            // Act
            var result = await _controller.GetCategory(TestUserConstants.UnitTestCategoryId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(category, okResult.Value);
        }


        [Fact]
        public async Task GetCategories_ReturnsOk()
        {
            // Arrange
            var categories = new List<CategoryDto>
            {
                new CategoryDto { Id = TestUserConstants.UnitTestCategoryId, Title = TestUserConstants.UnitTestTitle, Description = TestUserConstants.UnitTestDescription }
            };
            _categoryServiceMock.Setup(x => x.GetCategoriesAsync())
                                .ReturnsAsync(categories);

            // Act
            var result = await _controller.GetCategories();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(categories, okResult.Value);
        }
    }
}
