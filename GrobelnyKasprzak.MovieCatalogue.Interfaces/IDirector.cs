namespace GrobelnyKasprzak.MovieCatalogue.Interfaces
{
    public interface IDirector
    {
        int Id { get; set; }
        string Name { get; set; }
        int BirthYear { get; set; }
    }
}
