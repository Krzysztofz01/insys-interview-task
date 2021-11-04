using AutoMapper;
using MovieLibrary.Data.Entities;
using System.Linq;
using static MovieLibrary.Core.Dto.MovieDtos;

namespace MovieLibrary.Core.Mapping
{
    public class MoviesMapping : Profile
    {
        public MoviesMapping()
        {
            CreateMap<Movie, MovieSimple>();

            CreateMap<Movie, MovieDetails>()
                 .ForMember(d => d.Categories, m => m.MapFrom(s => s.MovieCategories.Select(e => e.Category)));
        }
    }
}
