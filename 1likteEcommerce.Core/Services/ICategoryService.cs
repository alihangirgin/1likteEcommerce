﻿using _1likteEcommerce.Core.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1likteEcommerce.Core.Services
{
    public interface ICategoryService
    {
        Task AddCategoryAsync(CategoryCreateDto model);
        Task UpdateCategoryAsync(int id, CategoryCreateDto model);
        Task<CategoryDto?> GetCategoryAsync(int id);
        Task<IEnumerable<CategoryDto>> GetCategoriesAsync();
        Task DeletCategoryAsync(int id);
    }
}