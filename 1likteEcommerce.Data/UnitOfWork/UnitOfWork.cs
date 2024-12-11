using _1likteEcommerce.Core.Repositories;
using _1likteEcommerce.Core.UnitOfWork;
using _1likteEcommerce.Data.DataAccess;
using _1likteEcommerce.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1likteEcommerce.Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _dbContext;
        private BasketRepository _basketRepository;
        private CategoryRepository _categoryRepository;
        private ProductRepository _productRepository;

        public UnitOfWork(AppDbContext appDbContext)
        {
            _dbContext = appDbContext;
        }

        public IBasketRepository Baskets => _basketRepository = _basketRepository ?? new BasketRepository(_dbContext);
        public ICategoryRepository Categories => _categoryRepository = _categoryRepository ?? new CategoryRepository(_dbContext);
        public IProductRepository Products => _productRepository = _productRepository ?? new ProductRepository(_dbContext);

        public async Task<int> Commit()
        {
            return await _dbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
