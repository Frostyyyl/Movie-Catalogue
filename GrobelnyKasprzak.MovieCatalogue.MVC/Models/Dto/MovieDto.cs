using GrobelnyKasprzak.MovieCatalogue.Core;
using GrobelnyKasprzak.MovieCatalogue.Interfaces;

namespace GrobelnyKasprzak.MovieCatalogue.MVC.Models.Dto;

internal class MovieDto : IMovie
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public int Year { get; set; }
    public MovieGenre Genre { get; set; }
    public int DirectorId { get; set; }
}

