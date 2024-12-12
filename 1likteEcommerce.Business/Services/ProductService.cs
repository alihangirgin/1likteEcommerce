using _1likteEcommerce.Core.Dtos;
using _1likteEcommerce.Core.Models;
using _1likteEcommerce.Core.Services;
using _1likteEcommerce.Core.UnitOfWork;

namespace _1likteEcommerce.Business.Services
{
    public class ProductService : IProductService
    {

        private readonly IUnitOfWork _unitOfWork;

        public ProductService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> AddProductAsync(ProductCreateDto model)
        {
            var checkCategory = await _unitOfWork.Categories.CheckAsync(model.CategoryId);
            if (!checkCategory) return false;

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
            return true;
        }

        public async Task<bool> UpdateProductAsync(int id, ProductCreateDto model)
        {
            var productEntity = await _unitOfWork.Products.GetByIdAsync(id);
            if (productEntity == null) return false;

            var checkCategory = await _unitOfWork.Categories.CheckAsync(model.CategoryId);
            if (!checkCategory) return false;

            productEntity.Description = model.Description;
            productEntity.Title = model.Title;
            productEntity.Price = model.Price;
            productEntity.CategoryId = model.CategoryId;

            await _unitOfWork.Products.UpdateAsync(productEntity);
            await _unitOfWork.Commit();

            return true;
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

        public async Task<bool> DeleteProductAsync(int id)
        {
            var productEntity = await _unitOfWork.Products.GetByIdAsync(id);
            if (productEntity == null) return false;

            await _unitOfWork.Products.DeleteAsync(id);
            await _unitOfWork.Commit();

            return true;
        }
    }
}