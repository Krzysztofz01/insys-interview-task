using AutoMapper;
using MovieLibrary.Core.Contracts;
using MovieLibrary.Data.Contracts;
using MovieLibrary.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static MovieLibrary.Core.Dto.MovieDtos;

namespace MovieLibrary.Core
{
    public class MovieService : IMovieService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MovieService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork ??
                throw new ArgumentNullException(nameof(unitOfWork));

            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }

        public async Task AddMovie(MovieCreate movie)
        {
            var categories = await _unitOfWork.CategoryRepository
                .GetCategoriesAsync(c => movie.CategoryIds.Contains(c.Id));

            var movieCategories = categories
                .Select(c => new MovieCategory { CategoryId = c.Id })
                .ToList();

            await _unitOfWork.MovieRepository.InsertMovieAsync(new Movie
            {
                Description = movie.Description,
                ImdbRating = movie.ImdbRating,
                Title = movie.Title,
                Year = movie.Year,
                MovieCategories = movieCategories
            });

            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteMovie(int movieId)
        {
            var movie = await _unitOfWork.MovieRepository.GetMovieByIdAsync(movieId);

            _unitOfWork.MovieRepository.DeleteMovie(movie);

            await _unitOfWork.SaveAsync();
        }

        public async Task<IEnumerable<MovieDetails>> GetAllMovies()
        {
            var movies = await _unitOfWork.MovieRepository.GetMoviesAsync();

            return _mapper.Map<IEnumerable<MovieDetails>>(movies);
        }

        public IEnumerable<MovieDetails> FilterByCategory(IEnumerable<MovieDetails> movies, string[] categoryIds)
        {
            return movies.Where(m => m.Categories.Select(s => s.Id.ToString()).Any(c => categoryIds.Contains(c)));
        }

        public IEnumerable<MovieDetails> FilterByKeyword(IEnumerable<MovieDetails> movies, string keyword)
        {
            return movies.Where(m => m.Title.ToLower().Contains(keyword.ToLower()));
        }

        public IEnumerable<MovieDetails> FilterByImdbRange(IEnumerable<MovieDetails> movies, decimal minImdb, decimal maxImdb)
        {
            return movies.Where(m => m.ImdbRating >= minImdb && m.ImdbRating <= maxImdb);
        }

        public async Task<MovieDetails> GetMovieById(int movieId)
        {
            var movie = await _unitOfWork.MovieRepository
                .GetMovieByIdAsync(movieId);

            return _mapper.Map<MovieDetails>(movie);
        }

        public async Task UpdateMovie(int movieId, MovieCreate movie)
        {
            var movieEntity = await _unitOfWork.MovieRepository
                .GetMovieByIdAsync(movieId);

            var categories = await _unitOfWork.CategoryRepository
                .GetCategoriesAsync(c => movie.CategoryIds.Contains(c.Id));

            var movieCategories = categories
                .Select(c => new MovieCategory { CategoryId = c.Id })
                .ToList();

            movieEntity.Description = movie.Description;
            movieEntity.ImdbRating = movie.ImdbRating;
            movieEntity.Title = movie.Title;
            movieEntity.Year = movie.Year;
            movieEntity.MovieCategories = movieCategories;

            _unitOfWork.MovieRepository.UpdateMovie(movieEntity);

            await _unitOfWork.SaveAsync();
        }
    }
}
