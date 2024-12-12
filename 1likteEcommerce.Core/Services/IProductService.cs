using _1likteEcommerce.Core.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1likteEcommerce.Core.Services
{
    public interface IProductService
    {
        Task<bool> AddProductAsync(ProductCreateDto model);
        Task<bool> UpdateProductAsync(int id, ProductCreateDto model);
        Task<ProductDto?> GetProductAsync(int id);
        Task<IEnumerable<ProductDto>> GetProductsAsync();
        Task<bool> DeleteProductAsync(int id);
    }
}
