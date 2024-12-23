﻿using _1likteEcommerce.Core.Models;
using _1likteEcommerce.Core.Repositories;
using _1likteEcommerce.Data.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace _1likteEcommerce.Data.Repositories
{
    public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity
    {
        private readonly AppDbContext _dbContext;
        protected readonly DbSet<TEntity> _dbSet;

        protected Repository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<TEntity>();
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            entity.CreatedAt = DateTime.Now;
            await _dbSet.AddAsync(entity);
            return entity;
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<TEntity?> GetByIdAsync(int id, string? include = null)
        {
            var query = _dbSet.AsQueryable();
            if (!string.IsNullOrEmpty(include))
            {
                query = query.Include(include);
            }
            return await query.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<TEntity?> UpdateAsync(TEntity entity)
        {
            var existingEntity = await _dbSet.FirstOrDefaultAsync(x => x.Id == entity.Id);
            if (existingEntity == null)
                return null;

            entity.UpdatedAt = DateTime.Now;
            _dbSet.Update(entity);
            //_dbContext.Entry(existingEntity).CurrentValues.SetValues(entity);
            return existingEntity;  
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
                _dbSet.Remove(entity);
        }

        public async Task<bool> CheckAsync(int id)
        {
            return await _dbSet.AnyAsync(x=> x.Id == id);
        }
    }
}
