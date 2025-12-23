namespace GrobelnyKasprzak.MovieCatalogue.Interfaces;

public interface IMovieService
{
    IEnumerable<IMovie> GetAllMovies();
    IEnumerable<IMovie> GetMoviesByDirectorId(int directorId);
    IMovie? GetMovieById(int id);
    void AddMovie(IMovie movie);
    void UpdateMovie(IMovie movie);
    void DeleteMovie(int id);
    IMovie CreateNewMovie();
}
