using _1likteEcommerce.Api.Controllers;
using _1likteEcommerce.Core.Constants;
using _1likteEcommerce.Core.Dtos;
using _1likteEcommerce.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;
using Xunit;

namespace _1likteEcommerce.Api.UnitTests
{
    public class BasketControllerTests
    {
        private readonly Mock<IBasketService> _basketServiceMock;
        private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private readonly BasketController _controller;

        public BasketControllerTests()
        {
            _basketServiceMock = new Mock<IBasketService>();
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();

           
            var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, TestUserConstants.UnitTestUserId)
            }));

            var httpContext = new DefaultHttpContext { User = user };
            _httpContextAccessorMock.Setup(x => x.HttpContext).Returns(httpContext);

            _controller = new BasketController(_basketServiceMock.Object, _httpContextAccessorMock.Object);
        }

        [Fact]
        public async Task AddProductToBasket_ReturnsOk()
        {
            // Arrange
            var model = new BasketAddProductDto { ProductId = TestUserConstants.UnitTestProductId };
            _basketServiceMock.Setup(x => x.AddProductToBasketAsync(It.IsAny<string>(), model))
                              .ReturnsAsync(true);

            // Act
            var result = await _controller.AddProductToBasket(model);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("product added to basket", okResult.Value);
        }

        [Fact]
        public async Task AddProductToBasket_ReturnsFailed()
        {
            // Arrange
            var model = new BasketAddProductDto { ProductId = 99999 };
            _basketServiceMock.Setup(x => x.AddProductToBasketAsync(It.IsAny<string>(), model))
                              .ReturnsAsync(false);

            // Act
            var result = await _controller.AddProductToBasket(model);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("product cannot added to basket", badRequestResult.Value);
        }

        [Fact]
        public async Task RemoveProductFromBasket_ReturnsOk()
        {
            // Arrange
            var model = new BasketAddProductDto { ProductId = TestUserConstants.UnitTestProductId };
            _basketServiceMock.Setup(x => x.RemoveProductFromBasketAsync(TestUserConstants.UnitTestUserId, model))
                              .ReturnsAsync(true);

            // Act
            var result = await _controller.RemoveProductFromBasket(model);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("product removed from the basket", okResult.Value);
        }

        [Fact]
        public async Task RemoveProductFromBasket_ReturnsFailed()
        {
            // Arrange
            var model = new BasketAddProductDto { ProductId = 99999 };
            _basketServiceMock.Setup(x => x.RemoveProductFromBasketAsync(TestUserConstants.UnitTestUserId, model))
                              .ReturnsAsync(false);

            // Act
            var result = await _controller.RemoveProductFromBasket(model);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("product cannot removed from the basket", badRequestResult.Value);
        }

        [Fact]
        public async Task GetBasket_ReturnsBasket()
        {
            // Arrange
            var basket = new BasketDto { Id = 1, UserId = TestUserConstants.UnitTestUserId };
            _basketServiceMock.Setup(x => x.GetBasketAsync(TestUserConstants.UnitTestUserId))
                              .ReturnsAsync(basket);

            // Act
            var result = await _controller.GetBasket();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(basket, okResult.Value);
        }
    }
}
