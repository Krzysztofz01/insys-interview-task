using MovieLibrary.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MovieLibrary.Data.Contracts
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetCategoriesAsync();
        Task<IEnumerable<Category>> GetCategoriesAsync(Expression<Func<Category, bool>> predicate);
        Task<Category> GetCategoryByIdAsync(int id);
        Task InsertCategoryAsync(Category category);
        void DeleteCategory(Category category);
        void UpdateCategory(Category category);
    }
}
