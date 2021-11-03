using System.Collections.Generic;
using static MovieLibrary.Api.Dto.MovieDtos;

namespace MovieLibrary.Api.Dto
{
    public static class CategoryDtos
    {
        public class CategorySimple
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        public class CategoryDetails
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public IEnumerable<MovieSimple> Movies { get; set; }
        }
    }
}
