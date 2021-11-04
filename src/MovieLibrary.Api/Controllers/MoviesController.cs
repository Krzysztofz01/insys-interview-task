using Microsoft.AspNetCore.Mvc;
using MovieLibrary.Core.Contracts;
using System;
using System.Linq;
using System.Threading.Tasks;
using static MovieLibrary.Core.Dto.MovieDtos;

namespace MovieLibrary.Api.Controllers
{
    [Route("v{version:apiVersion}/MovieManagement")]
    [ApiVersion("1.0")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieService _movieService;

        public MoviesController(IMovieService movieService)
        {
            _movieService = movieService ??
                throw new ArgumentNullException(nameof(movieService));
        }

        [HttpGet]
        public async Task<IActionResult> GetMovies([FromQuery] string text, [FromQuery] string categories, [FromQuery] decimal? minImdb, [FromQuery] decimal? maxImdb)
        {
            var movies = await _movieService.GetAllMovies();

            if (text != null) movies = _movieService.FilterByKeyword(movies, text);

            if (categories != null) movies = _movieService.FilterByCategory(movies, categories.Split(','));

            if (maxImdb.HasValue && minImdb.HasValue) movies = _movieService.FilterByImdbRange(movies, minImdb.Value, maxImdb.Value);

            if (!movies.Any()) return NotFound();

            return Ok(movies);
        }

        [HttpPost]
        public async Task<IActionResult> PostMovie(MovieCreate movie)
        {
            await _movieService.AddMovie(movie);

            return Ok();
        }

        [HttpGet("{movieId}")]
        public async Task<IActionResult> GetMovie(int movieId)
        {
            var movie = await _movieService.GetMovieById(movieId);
            
            if (movie is null) return NotFound();

            return Ok(movie);
        }

        [HttpDelete("{movieId}")]
        public async Task<IActionResult> DeleteMovie(int movieId)
        {
            await _movieService.DeleteMovie(movieId);

            return Ok();
        }

        [HttpPut("{movieId}")]
        public async Task<IActionResult> PutMovie(int movieId, [FromBody]MovieCreate movie)
        {
            await _movieService.UpdateMovie(movieId, movie);

            return Ok();
        }
    }
}
