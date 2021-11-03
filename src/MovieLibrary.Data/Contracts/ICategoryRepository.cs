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
        Task InsertCategoryAsync(Category movie);
        void DeleteCategory(Category movie);
        void UpdateCategory(Category movie);
    }
}
