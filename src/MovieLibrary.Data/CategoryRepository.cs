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

        public void DeleteCategory(Category movie)
        {
            _context.Categories.Remove(movie);
        }

        public async Task<IEnumerable<Category>> GetCategoriesAsync()
        {
            return await _context.Categories
                .Include(c => c.MovieCategories)
                .ThenInclude(c => c.Category)
                .ToListAsync();
        }

        public async Task<IEnumerable<Category>> GetCategoriesAsync(Expression<Func<Category, bool>> predicate)
        {
            return await _context.Categories
                .Include(c => c.MovieCategories)
                .ThenInclude(c => c.Category)
                .Where(predicate)
                .ToListAsync();
        }

        public async Task<Category> GetCategoryByIdAsync(int id)
        {
            return await _context.Categories
                .Include(c => c.MovieCategories)
                .ThenInclude(c => c.Category)
                .SingleOrDefaultAsync(c => c.Id == id);
        }

        public async Task InsertCategoryAsync(Category movie)
        {
            await _context.Categories.AddAsync(movie);
        }

        public void UpdateCategory(Category movie)
        {
            _context.Entry(movie).State = EntityState.Modified;
        }
    }
}
