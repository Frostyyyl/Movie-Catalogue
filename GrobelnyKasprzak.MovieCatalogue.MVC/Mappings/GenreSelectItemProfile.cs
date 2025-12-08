using AutoMapper;
using GrobelnyKasprzak.MovieCatalogue.Core;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GrobelnyKasprzak.MovieCatalogue.MVC.Mappings;

public class GenreSelectItemProfile : Profile
{
    public GenreSelectItemProfile()
    {
        CreateMap<MovieGenre, SelectListItem>()
            .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.ToString()))
            .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.ToString()));
    }
}