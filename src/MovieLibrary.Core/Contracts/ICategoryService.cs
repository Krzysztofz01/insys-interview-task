using System.Collections.Generic;
using System.Threading.Tasks;
using static MovieLibrary.Core.Dto.CategoryDtos;

namespace MovieLibrary.Core.Contracts
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDetails>> GetAllCategories();
        Task<CategoryDetails> GetCategoryById(int categoryId);
        Task AddCategory(CategoryRequest category);
        Task UpdateCategory(int categoryId, CategoryRequest category);
        Task DeleteCategory(int categoryId);
    }
}
