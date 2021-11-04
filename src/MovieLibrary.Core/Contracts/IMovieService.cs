using System.Collections.Generic;
using System.Threading.Tasks;
using static MovieLibrary.Core.Dto.MovieDtos;

namespace MovieLibrary.Core.Contracts
{
    public interface IMovieService
    {
        Task<IEnumerable<MovieDetails>> GetAllMovies();

        IEnumerable<MovieDetails> FilterByKeyword(IEnumerable<MovieDetails> movies, string keyword);
        IEnumerable<MovieDetails> FilterByCategory(IEnumerable<MovieDetails> movies, string[] categoryIds);
        IEnumerable<MovieDetails> FilterByImdbRange(IEnumerable<MovieDetails> movies, decimal minImdb, decimal maxImdb);

        Task<MovieDetails> GetMovieById(int movieId);
        Task AddMovie(MovieRequest movie);
        Task UpdateMovie(int movieId, MovieRequest movie);
        Task DeleteMovie(int movieId);
    }
}
