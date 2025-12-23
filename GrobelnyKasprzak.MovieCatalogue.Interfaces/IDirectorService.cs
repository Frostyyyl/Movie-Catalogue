namespace GrobelnyKasprzak.MovieCatalogue.Interfaces;

public interface IDirectorService
{
    IEnumerable<IDirector> GetAllDirectors();
    IDirector? GetDirectorById(int id);
    void AddDirector(IDirector director);
    void UpdateDirector(IDirector director);
    void DeleteDirector(int id);
    IDirector CreateNewDirector();
}