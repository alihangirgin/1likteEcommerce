using _1likteEcommerce.Core.Models;
using _1likteEcommerce.Core.Repositories;
using _1likteEcommerce.Data.DataAccess;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1likteEcommerce.Data.Repositories
{
    public class BasketRepository : Repository<Basket>, IBasketRepository
    {
        public BasketRepository(AppDbContext dbContext) : base(dbContext)
        {
        }
        public async Task<Basket?> GetBasketByUserId(string userId)
        {
            return await _dbSet.Include(x=> x.BasketItems).FirstOrDefaultAsync(x => x.UserId == userId);
        }
    }
}
