namespace GrobelnyKasprzak.MovieCatalogue.MVC.ViewModels;

public class DirectorViewModel
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public ICollection<MovieListItemViewModel> Movies { get; set; } = [];
}

