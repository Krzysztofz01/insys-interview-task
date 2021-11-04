using Microsoft.AspNetCore.Mvc;
using MovieLibrary.Core.Contracts;
using System;
using System.Linq;
using System.Threading.Tasks;
using static MovieLibrary.Core.Dto.CategoryDtos;

namespace MovieLibrary.Api.Controllers
{
    [Route("v{version:apiVersion}/CategoryManagement")]
    [ApiVersion("1.0")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService ??
                throw new ArgumentNullException(nameof(categoryService));
        }

        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _categoryService.GetAllCategories();

            if (!categories.Any()) return NotFound();

            return Ok(categories);
        }

        [HttpPost]
        public async Task<IActionResult> PostCategory(CategoryRequest category)
        {
            await _categoryService.AddCategory(category);

            return Ok();
        }

        [HttpGet("{categoryId}")]
        public async Task<IActionResult> GetCategory(int categoryId)
        {
            var category = await _categoryService.GetCategoryById(categoryId);

            if (category is null) return NotFound();

            return Ok(category);
        }

        [HttpDelete("categoryId")]
        public async Task<IActionResult> DeleteCategory(int categoryId)
        {
            await _categoryService.DeleteCategory(categoryId);

            return Ok();
        }

        [HttpPut("categoryId")]
        public async Task<IActionResult> PutCategory(int categoryId, [FromBody]CategoryRequest category)
        {
            await _categoryService.UpdateCategory(categoryId, category);

            return Ok();
        }
    }
}
