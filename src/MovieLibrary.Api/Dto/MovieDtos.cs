using System.Collections.Generic;
using static MovieLibrary.Api.Dto.CategoryDtos;

namespace MovieLibrary.Api.Dto
{
    public static class MovieDtos
    {
        public class MovieSimple
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public int Year { get; set; }
            public decimal ImdbRating { get; set; }
        }

        public class MovieDetails
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public int Year { get; set; }
            public decimal ImdbRating { get; set; }
            public IEnumerable<CategorySimple> Categories { get; set; }
        }
    }
}
