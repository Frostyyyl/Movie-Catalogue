using AutoMapper;
using GrobelnyKasprzak.MovieCatalogue.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GrobelnyKasprzak.MovieCatalogue.MVC.Mappings;

public class DirectorSelectItemProfile : Profile
{
    public DirectorSelectItemProfile()
    {
        CreateMap<IDirector, SelectListItem>()
            .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Name));
    }
}
