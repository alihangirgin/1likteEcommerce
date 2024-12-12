using _1likteEcommerce.Core.Dtos;
using _1likteEcommerce.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace _1likteEcommerce.Api.Controllers
{
    [Authorize]
    [Route("api/categories")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory(CategoryCreateDto model)
        {
            if (ModelState.IsValid)
            {
                await _categoryService.AddCategoryAsync(model);
                return Ok("category created");
            }

            return BadRequest(ModelState);
        }

        [HttpPut("{categoryId}")]
        public async Task<IActionResult> UpdateCategory(int categoryId, CategoryCreateDto model)
        {
            if (ModelState.IsValid)
            {
                await _categoryService.UpdateCategoryAsync(categoryId, model);
                return Ok("category created");
            }

            return BadRequest(ModelState);
        }

        [HttpDelete("{categoryId}")]
        public async Task<IActionResult> DeleteCategory(int categoryId)
        {
            await _categoryService.DeleteCategoryAsync(categoryId);
            return Ok("category deleted");
        }

        [HttpGet("{categoryId}")]
        public async Task<IActionResult> GetCategory(int categoryId)
        {
            var category = await _categoryService.GetCategoryAsync(categoryId);
            if(category == null)
                return NotFound("category not found");
            return Ok(category);
        }

        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _categoryService.GetCategoriesAsync();
            if (categories == null)
                return NotFound("categories not found");
            return Ok(categories);
        }
    }
}
