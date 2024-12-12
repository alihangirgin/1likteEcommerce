using _1likteEcommerce.Core.Dtos;
using _1likteEcommerce.Core.Models;
using _1likteEcommerce.Core.Services;
using _1likteEcommerce.Core.UnitOfWork;

namespace _1likteEcommerce.Business.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> AddCategoryAsync(CategoryCreateDto model)
        {
            Category categoryEntity = new()
            {
                Description = model.Description,
                Title = model.Title,
                Products = new List<Product>()
            };
            await _unitOfWork.Categories.AddAsync(categoryEntity);
            await _unitOfWork.Commit();

            return true;
        }

        public async Task<bool> UpdateCategoryAsync(int id, CategoryCreateDto model)
        {
            var categoryEntity = await _unitOfWork.Categories.GetByIdAsync(id);
            if (categoryEntity == null) return false;

            categoryEntity.Description = model.Description;
            categoryEntity.Title = model.Title;
 
            await _unitOfWork.Categories.UpdateAsync(categoryEntity);
            await _unitOfWork.Commit();
            return true;
        }

        public async Task<CategoryDto?> GetCategoryAsync(int id)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(id);
            if (category == null) return null;
            return new CategoryDto()
            {
                Id = id,
                Title = category.Title,
                Description = category.Description,
                CreatedAt = category.CreatedAt,
                UpdatedAt = category.UpdatedAt,
            };
        }

        public async Task<IEnumerable<CategoryDto>> GetCategoriesAsync()
        {
            var categories = await _unitOfWork.Categories.GetAllAsync();
            return categories.Select(x => new CategoryDto()
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
            }).ToList();
        }

        public async Task<bool> DeleteCategoryAsync(int id)
        {
            var categoryEntity = await _unitOfWork.Categories.GetByIdAsync(id);
            if (categoryEntity == null) return false;

            await _unitOfWork.Categories.DeleteAsync(id);
            await _unitOfWork.Commit();

            return true;
        }
    }
}
