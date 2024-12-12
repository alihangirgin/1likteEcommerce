using _1likteEcommerce.Api.Controllers;
using _1likteEcommerce.Core.Constants;
using _1likteEcommerce.Core.Dtos;
using _1likteEcommerce.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace _1likteEcommerce.Api.UnitTests
{
    public class ProductControllerTests
    {
        private readonly Mock<IProductService> _productServiceMock;
        private readonly ProductController _controller;

        public ProductControllerTests()
        {
            _productServiceMock = new Mock<IProductService>();
            _controller = new ProductController(_productServiceMock.Object);
        }

        [Fact]
        public async Task CreateProduct_ReturnsOk()
        {
            // Arrange
            var model = new ProductCreateDto
            {
                Title = TestUserConstants.UnitTestTitle,
                Description = TestUserConstants.UnitTestDescription,
                Price = 100,
                CategoryId = TestUserConstants.UnitTestCategoryId
            };
            _productServiceMock.Setup(x => x.AddProductAsync(model))
                               .ReturnsAsync(true);

            // Act
            var result = await _controller.CreateProduct(model);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("product created", okResult.Value);
        }

        [Fact]
        public async Task CreateProduct_ReturnsFailed()
        {
            // Arrange
            var model = new ProductCreateDto
            {
                Title = TestUserConstants.UnitTestTitle,
                Description = TestUserConstants.UnitTestDescription,
                Price = 100,
                CategoryId = 999999
            };
            _productServiceMock.Setup(x => x.AddProductAsync(model))
                               .ReturnsAsync(false);

            // Act
            var result = await _controller.CreateProduct(model);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("product cannot created", badRequestResult.Value);
        }

        [Fact]
        public async Task UpdateProduct_ReturnsOk()
        {
            // Arrange
            var model = new ProductCreateDto
            {
                Title = TestUserConstants.UnitTestTitle,
                Description = TestUserConstants.UnitTestDescription,
                Price = 100,
                CategoryId = TestUserConstants.UnitTestCategoryId
            };
            _productServiceMock.Setup(x => x.UpdateProductAsync(TestUserConstants.UnitTestProductId, model))
                               .ReturnsAsync(true);

            // Act
            var result = await _controller.UpdateProduct(TestUserConstants.UnitTestProductId, model);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("product updated", okResult.Value);
        }

        [Fact]
        public async Task UpdateProduct_ReturnsFailed()
        {
            // Arrange
            var model = new ProductCreateDto
            {
                Title = TestUserConstants.UnitTestTitle,
                Description = TestUserConstants.UnitTestDescription,
                Price = 100,
                CategoryId = 999999
            };
            _productServiceMock.Setup(x => x.UpdateProductAsync(TestUserConstants.UnitTestProductId, model))
                               .ReturnsAsync(false);

            // Act
            var result = await _controller.UpdateProduct(999999, model);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("product cannot updated", badRequestResult.Value);
        }

        [Fact]
        public async Task GetProduct_ReturnsOk()
        {
            // Arrange
            var product = new ProductDto
            {
                Id = TestUserConstants.UnitTestProductId,
                Title = TestUserConstants.UnitTestTitle,
                Description = TestUserConstants.UnitTestDescription,
                Price = 100,
                CategoryId = TestUserConstants.UnitTestCategoryId
            };
            _productServiceMock.Setup(x => x.GetProductAsync(TestUserConstants.UnitTestProductId))
                               .ReturnsAsync(product);

            // Act
            var result = await _controller.GetProduct(TestUserConstants.UnitTestProductId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(product, okResult.Value);
        }


        [Fact]
        public async Task GetProducts_ReturnsOk()
        {
            // Arrange
            var products = new List<ProductDto>
            {
                new ProductDto
                {
                    Id = TestUserConstants.UnitTestProductId,
                    Title = TestUserConstants.UnitTestTitle,
                    Description = TestUserConstants.UnitTestDescription,
                    Price = 100.00m,
                    CategoryId = TestUserConstants.UnitTestCategoryId
                }
            };
            _productServiceMock.Setup(x => x.GetProductsAsync())
                               .ReturnsAsync(products);

            // Act
            var result = await _controller.GetProducts();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(products, okResult.Value);
        }
    }
}
