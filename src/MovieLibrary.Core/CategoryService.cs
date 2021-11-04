using AutoMapper;
using MovieLibrary.Core.Contracts;
using MovieLibrary.Data.Contracts;
using MovieLibrary.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static MovieLibrary.Core.Dto.CategoryDtos;

namespace MovieLibrary.Core
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CategoryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork ??
                throw new ArgumentNullException(nameof(unitOfWork));

            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }

        public async Task AddCategory(CategoryRequest category)
        {
            var movies = await _unitOfWork.MovieRepository
                .GetMoviesAsync(m => category.MovieIds.Contains(m.Id));

            var movieCategories = movies
                .Select(m => new MovieCategory { MovieId = m.Id })
                .ToList();

            await _unitOfWork.CategoryRepository.InsertCategoryAsync(new Category
            {
                Name = category.Name,
                MovieCategories = movieCategories
            });

            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteCategory(int categoryId)
        {
            var category = await _unitOfWork.CategoryRepository.GetCategoryByIdAsync(categoryId);

            _unitOfWork.CategoryRepository.DeleteCategory(category);

            await _unitOfWork.SaveAsync();
        }

        public async Task<IEnumerable<CategoryDetails>> GetAllCategories()
        {
            var categories = await _unitOfWork.CategoryRepository.GetCategoriesAsync();

            return _mapper.Map<IEnumerable<CategoryDetails>>(categories);
        }

        public async Task<CategoryDetails> GetCategoryById(int categoryId)
        {
            var category = await _unitOfWork.CategoryRepository.GetCategoryByIdAsync(categoryId);

            return _mapper.Map<CategoryDetails>(category);
        }

        public async Task UpdateCategory(int categoryId, CategoryRequest category)
        {
            var categoryEntity = await _unitOfWork.CategoryRepository.GetCategoryByIdAsync(categoryId);

            var movies = await _unitOfWork.MovieRepository.
                GetMoviesAsync(m => category.MovieIds.Contains(m.Id));

            var movieCategories = movies
                .Select(m => new MovieCategory { MovieId = m.Id })
                .ToList();

            categoryEntity.Name = category.Name;
            categoryEntity.MovieCategories = movieCategories;

            _unitOfWork.CategoryRepository.UpdateCategory(categoryEntity);

            await _unitOfWork.SaveAsync();
        }
    }
}
