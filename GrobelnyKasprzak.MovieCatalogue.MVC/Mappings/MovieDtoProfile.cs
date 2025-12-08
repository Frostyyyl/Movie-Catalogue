using AutoMapper;
using GrobelnyKasprzak.MovieCatalogue.MVC.Models.Dto;
using GrobelnyKasprzak.MovieCatalogue.MVC.ViewModels;

namespace GrobelnyKasprzak.MovieCatalogue.MVC.Mappings;

public class MovieDtoProfile : Profile
{
    public MovieDtoProfile()
    {
        CreateMap<MovieViewModel, MovieDto>();
    }
}
