using AutoMapper;
using GrobelnyKasprzak.MovieCatalogue.Core;
using GrobelnyKasprzak.MovieCatalogue.Interfaces;
using GrobelnyKasprzak.MovieCatalogue.MVC.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GrobelnyKasprzak.MovieCatalogue.MVC.Mappings;

public class MovieProfile : Profile
{
    public MovieProfile()
    {
        CreateMap<IMovie, MovieViewModel>()
            .ForMember(dest => dest.Director, opt => opt.MapFrom((src, dest, _, ctx)
                => ctx.Items.TryGetValue(MappingKeys.DirectorName, out var name)
                    ? name : "Unknown"))
            .ForMember(dest => dest.AvailableDirectors, opt => opt.MapFrom((src, dest, _, ctx) =>
            {
                if (ctx.Items.TryGetValue(MappingKeys.AvailableDirectors, out var listObj) &&
                    listObj is IEnumerable<IDirector> directors)
                {
                    return ctx.Mapper.Map<IEnumerable<SelectListItem>>(directors);
                }
                return [];
            }))
            .ForMember(dest => dest.AvailableGenres, opt => opt.MapFrom((src, dest, _, ctx) =>
            {
                if (ctx.Items.TryGetValue(MappingKeys.AvailableGenres, out var listObj) &&
                    listObj is IEnumerable<MovieGenre> genres)
                {
                    return ctx.Mapper.Map<IEnumerable<SelectListItem>>(genres);
                }
                return [];
            }));
    }
}
