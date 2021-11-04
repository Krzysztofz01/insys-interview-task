using Microsoft.EntityFrameworkCore;
using MovieLibrary.Data.Contracts;
using MovieLibrary.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MovieLibrary.Data
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly MovieLibraryContext _context;

        public CategoryRepository(MovieLibraryContext movieLibraryContext)
        {
            _context = movieLibraryContext ??
                throw new ArgumentNullException(nameof(movieLibraryContext));
        }

        public void DeleteCategory(Category category)
        {
            _context.Categories.Remove(category);
        }

        public async Task<IEnumerable<Category>> GetCategoriesAsync()
        {
            return await _context.Categories
                .Include(c => c.MovieCategories)
                .ThenInclude(c => c.Movie)
                .ToListAsync();
        }

        public async Task<IEnumerable<Category>> GetCategoriesAsync(Expression<Func<Category, bool>> predicate)
        {
            return await _context.Categories
                .Include(c => c.MovieCategories)
                .ThenInclude(c => c.Movie)
                .Where(predicate)
                .ToListAsync();
        }

        public async Task<Category> GetCategoryByIdAsync(int id)
        {
            return await _context.Categories
                .Include(c => c.MovieCategories)
                .ThenInclude(c => c.Movie)
                .SingleOrDefaultAsync(c => c.Id == id);
        }

        public async Task InsertCategoryAsync(Category category)
        {
            await _context.Categories.AddAsync(category);
        }

        public void UpdateCategory(Category category)
        {
            _context.Entry(category).State = EntityState.Modified;
        }
    }
}
