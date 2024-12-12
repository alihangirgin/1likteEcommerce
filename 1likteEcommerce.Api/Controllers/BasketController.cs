using _1likteEcommerce.Core.Dtos;
using _1likteEcommerce.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace _1likteEcommerce.Api.Controllers
{
    [Authorize]
    [Route("api/baskets")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IBasketService _basketService;
        private string userId = string.Empty;
        public BasketController(IBasketService basketService, IHttpContextAccessor httpContextAccessor)
        {
            _basketService = basketService;
            userId = httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
        }

        [HttpPost("add-product")]
        public async Task<IActionResult> AddProductToBasket(BasketAddProductDto model)
        {
            if (ModelState.IsValid)
            {
                var result = await _basketService.AddProductToBasketAsync(userId, model);
                if (!result) return BadRequest("product cannot added to basket");
                return Ok("product added to basket");
            }

            return BadRequest(ModelState);
        }

        [HttpPost("remove-product")]
        public async Task<IActionResult> RemoveProductFromBasket(BasketAddProductDto model)
        {
            if (ModelState.IsValid)
            {
                var result = await _basketService.RemoveProductFromBasketAsync(userId, model);
                if (!result) return BadRequest("product cannot removed from the basket");
                return Ok("product removed from the basket");
            }
             
            return BadRequest(ModelState);
        }

        [HttpGet]
        public async Task<IActionResult> GetBasket()
        {
            var basket = await _basketService.GetBasketAsync(userId);
            return Ok(basket);
        }
    }
}
