using AutoMapper;
using GrobelnyKasprzak.MovieCatalogue.Interfaces;
using GrobelnyKasprzak.MovieCatalogue.MVC.ViewModels;

namespace GrobelnyKasprzak.MovieCatalogue.MVC.Mappings;

public class MovieListItemProfile : Profile
{
    public MovieListItemProfile()
    {
        CreateMap<IMovie, MovieListItemViewModel>();
    }
}
