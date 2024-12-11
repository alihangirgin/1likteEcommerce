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
    public class ProductService : IProductService
    {

        private readonly IUnitOfWork _unitOfWork;

        public ProductService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task AddProductAsync(ProductCreateDto model)
        {
            Product productEntity = new()
            {
                Description = model.Description,
                Title = model.Title,
                CategoryId = model.CategoryId,
                Price = model.Price,
                BasketItems = new List<BasketItem>()
            };
            await _unitOfWork.Products.AddAsync(productEntity);
            await _unitOfWork.Commit();
        }

        public async Task UpdateProductAsync(int id, ProductCreateDto model)
        {
            var productEntity = await _unitOfWork.Products.GetByIdAsync(id);
            if (productEntity == null) return;

            productEntity.Description = model.Description;
            productEntity.Title = model.Title;
            productEntity.Price = model.Price;
            productEntity.CategoryId = model.CategoryId;

            await _unitOfWork.Products.UpdateAsync(productEntity);
            await _unitOfWork.Commit();
        }

        public async Task<ProductDto?> GetProductAsync(int id)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(id);
            if (product == null) return null;
            return new ProductDto()
            {
                Id = id,
                Title = product.Title,
                Description = product.Description,
                CreatedAt = product.CreatedAt,
                UpdatedAt = product.UpdatedAt,
                CategoryId = product.CategoryId
            };
        }

        public async Task<IEnumerable<ProductDto>> GetProductsAsync()
        {
            var products = await _unitOfWork.Products.GetAllAsync();
            return products.Select(x => new ProductDto()
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
                CategoryId = x.CategoryId
            }).ToList();
        }

        public async Task DeletProductAsync(int id)
        {
            await _unitOfWork.Products.DeleteAsync(id);
            await _unitOfWork.Commit();
        }
    }
}