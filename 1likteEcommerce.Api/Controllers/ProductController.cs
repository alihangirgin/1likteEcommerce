using _1likteEcommerce.Business.Services;
using _1likteEcommerce.Core.Dtos;
using _1likteEcommerce.Core.Models;
using _1likteEcommerce.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace _1likteEcommerce.Api.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(ProductCreateDto model)
        {
            if (ModelState.IsValid)
            {
                await _productService.AddProductAsync(model);
                return Ok("product created");
            }

            return BadRequest(ModelState);
        }

        [HttpPut("{productId}")]
        public async Task<IActionResult> UpdateProduct(int productId, ProductCreateDto model)
        {
            if (ModelState.IsValid)
            {
                await _productService.UpdateProductAsync(productId, model);
                return Ok("product created");
            }

            return BadRequest(ModelState);
        }

        [HttpDelete("{productId}")]
        public async Task<IActionResult> DeleteProduct(int productId)
        {
            await _productService.DeletProductAsync(productId);
            return Ok("product deleted");
        }

        [HttpGet("{productId}")]
        public async Task<IActionResult> GetProduct(int productId)
        {
            var product = await _productService.GetProductAsync(productId);
            if (product == null)
                return NotFound("product not found");
            return Ok(product);
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var products = await _productService.GetProductsAsync();
            if (products == null)
                return NotFound("products not found");
            return Ok(products);
        }
    }
}
