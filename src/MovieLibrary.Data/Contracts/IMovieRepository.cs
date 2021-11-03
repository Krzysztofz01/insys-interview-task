using MovieLibrary.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MovieLibrary.Data.Contracts
{
    public interface IMovieRepository
    {
        Task<IEnumerable<Movie>> GetMoviesAsync();
        Task<IEnumerable<Movie>> GetMoviesAsync(Expression<Func<Movie, bool>> predicate);
        Task<Movie> GetMovieByIdAsync(int id);
        Task InsertMovieAsync(Movie movie);
        void DeleteMovie(Movie movie);
        void UpdateMovie(Movie movie);
    }
}
