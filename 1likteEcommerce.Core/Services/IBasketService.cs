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
        Task AddProductToBasketAsync(int userId, BasketAddProductDto model);
        Task RemoveProductFromBasketAsync(int userId, BasketAddProductDto model);
        Task<BasketDto?> GetBasketAsync(int id);
    }
}
