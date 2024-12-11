using _1likteEcommerce.Core.Dtos;
using _1likteEcommerce.Core.Models;
using _1likteEcommerce.Core.Services;
using _1likteEcommerce.Core.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1likteEcommerce.Business.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task AddCategoryAsync(CategoryCreateDto model)
        {
            Category categoryEntity = new()
            {
                Description = model.Description,
                Title = model.Title,
                Products = new List<Product>()
            };
            await _unitOfWork.Categories.AddAsync(categoryEntity);
            await _unitOfWork.Commit();
        }

        public async Task UpdateCategoryAsync(int id, CategoryCreateDto model)
        {
            var categoryEntity = await _unitOfWork.Categories.GetByIdAsync(id);
            if (categoryEntity == null) return;

            categoryEntity.Description = model.Description;
            categoryEntity.Title = model.Title;
 
            await _unitOfWork.Categories.UpdateAsync(categoryEntity);
            await _unitOfWork.Commit();
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
                Products = category.Products.Select(x => new ProductDto()
                {
                    Id = x.Id,
                    CategoryId = x.CategoryId,
                    CreatedAt = x.CreatedAt,
                    UpdatedAt = x.UpdatedAt,
                    Description = x.Description,
                    Price = x.Price,
                    Title = x.Title 
                }).ToList()
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
                Products = x.Products.Select(x => new ProductDto()
                {
                    Id = x.Id,
                    CategoryId = x.CategoryId,
                    CreatedAt = x.CreatedAt,
                    UpdatedAt = x.UpdatedAt,
                    Description = x.Description,
                    Price = x.Price,
                    Title = x.Title
                }).ToList()
            }).ToList();
        }

        public async Task DeletCategoryAsync(int id)
        {
            await _unitOfWork.Categories.DeleteAsync(id);
            await _unitOfWork.Commit();
        }
    }
}
