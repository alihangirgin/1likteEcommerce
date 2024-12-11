using _1likteEcommerce.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1likteEcommerce.Core.Repositories
{
    public interface IRepository<TEntity> where TEntity : Entity
    {
        Task<TEntity> AddAsync(TEntity entity);
        Task<TEntity?> UpdateAsync(TEntity entity);
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<TEntity?> GetByIdAsync(int id, string? include = null);
        Task DeleteAsync(int id);
    }
}
