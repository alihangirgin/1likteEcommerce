using _1likteEcommerce.Core.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1likteEcommerce.Core.Services
{
    public interface IBasketService
    {
        Task<bool> AddProductToBasketAsync(string userId, BasketAddProductDto model);
        Task<bool> RemoveProductFromBasketAsync(string userId, BasketAddProductDto model);
        Task<BasketDto?> GetBasketAsync(string userId);
    }
}
