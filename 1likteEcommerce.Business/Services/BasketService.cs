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
    public class BasketService : IBasketService
    {
        private readonly IUnitOfWork _unitOfWork;

        public BasketService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> AddProductToBasketAsync(string userId, BasketAddProductDto model)
        {
            var basketEntity = await _unitOfWork.Baskets.GetBasketByUserId(userId);
            if (basketEntity == null) return false;

            var existingBasketItem = basketEntity.BasketItems.FirstOrDefault(x => x.ProductId == model.ProductId);
            if (existingBasketItem == null)
                basketEntity.BasketItems.Add(new BasketItem()
                {
                    ProductId = model.ProductId,
                    Quantity = 1
                });
            else
                existingBasketItem.Quantity++;

            await _unitOfWork.Baskets.UpdateAsync(basketEntity);
            await _unitOfWork.Commit();

            return true;
        }

        public async Task<bool> RemoveProductFromBasketAsync(string userId, BasketAddProductDto model)
        {
            var basketEntity = await _unitOfWork.Baskets.GetBasketByUserId(userId);
            if (basketEntity == null) return false;

            var existingBasketItem = basketEntity.BasketItems.FirstOrDefault(x => x.ProductId == model.ProductId);
            if (existingBasketItem == null) return false;

            if (existingBasketItem.Quantity <= 1)
                basketEntity.BasketItems.Remove(existingBasketItem);
            else
                existingBasketItem.Quantity--;

            await _unitOfWork.Baskets.UpdateAsync(basketEntity);
            await _unitOfWork.Commit();

            return true;
        }

        public async Task<BasketDto?> GetBasketAsync(string userId)
        {
            var basket = await _unitOfWork.Baskets.GetBasketWithProductsByUserId(userId);
            if (basket == null) return null;
            return new BasketDto()
            {
                Id = basket.Id,
                UpdatedAt = basket.UpdatedAt,
                UserId = basket.UserId,
                BasketItems = basket.BasketItems.Select(x => new BasketItemDto()
                {
                    Id = x.Id,
                    ProductId = x.ProductId,
                    Product = new ProductDto() { CategoryId = x.Product.CategoryId, Description = x.Product.Description, Id = x.Product.Id, Price = x.Product.Price, Title = x.Product.Title, CreatedAt = x.CreatedAt },
                    Quantity = x.Quantity,
                    UpdatedAt = x.UpdatedAt,
                    CreatedAt = x.CreatedAt,
                    BasketId = x.BasketId
                }).ToList()
            };
        }
    }
}
