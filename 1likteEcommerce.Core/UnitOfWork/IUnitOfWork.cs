using _1likteEcommerce.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1likteEcommerce.Core.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IBasketRepository Baskets {  get; }
        ICategoryRepository Categories { get; }
        IProductRepository Products { get; }
        Task<int> Commit();
    }
}
