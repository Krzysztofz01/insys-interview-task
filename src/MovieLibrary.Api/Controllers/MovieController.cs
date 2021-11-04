using Microsoft.AspNetCore.Mvc;
using MovieLibrary.Api.Utilities;
using MovieLibrary.Core.Contracts;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MovieLibrary.Api.Controllers
{
    [Route("v{version:apiVersion}/Movie")]
    [ApiVersion("1.0")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly IMovieService _movieService;

        public MovieController(IMovieService movieService)
        {
            _movieService = movieService ??
                throw new ArgumentNullException(nameof(movieService));
        }

        [HttpGet("Filter")]
        public async Task<IActionResult> GetMoviesFiltered([FromQuery] string text, [FromQuery] string categoriesId, [FromQuery] decimal? minImdb, [FromQuery] decimal? maxImdb, [FromQuery] int? page)
        {
            var movies = await _movieService.GetAllMovies();

            //Apply filters from query
            if (text != null) movies = _movieService.FilterByKeyword(movies, text);

            if (categoriesId != null) movies = _movieService.FilterByCategory(movies, categoriesId.Split(','));

            if (maxImdb.HasValue && minImdb.HasValue) movies = _movieService.FilterByImdbRange(movies, minImdb.Value, maxImdb.Value);

            //Return 404 if collection empty
            if (!movies.Any()) return NotFound();

            //Sort collection by Imdb rating according to specs
            var moviesInOrder = movies.OrderByDescending(m => m.ImdbRating);

            //If page is defined use pagination utility, if not return whole collection
            if (page != null) return Ok(PaginationUtility.Paginate(moviesInOrder, page.Value));

            return Ok(moviesInOrder);
        }
    }
}
