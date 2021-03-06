using AutoMapper;
using MovieLibrary.Data.Entities;
using System.Linq;
using static MovieLibrary.Core.Dto.CategoryDtos;

namespace MovieLibrary.Core.Mapping
{
    public class CategoryMapping : Profile
    {
        public CategoryMapping()
        {
            CreateMap<Category, CategorySimple>();

            CreateMap<Category, CategoryDetails>()
                .ForMember(d => d.Movies, m => m.MapFrom(s => s.MovieCategories.Select(c => c.Movie)));
        }
    }
}
