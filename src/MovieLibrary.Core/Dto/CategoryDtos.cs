using System.Collections.Generic;
using static MovieLibrary.Core.Dto.MovieDtos;

namespace MovieLibrary.Core.Dto
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

        public class CategoryCreate
        {
            public string Name { get; set; }
            public IEnumerable<int> MovieIds { get; set; }
        }
    }
}
