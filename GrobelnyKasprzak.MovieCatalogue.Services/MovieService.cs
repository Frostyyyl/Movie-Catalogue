using GrobelnyKasprzak.MovieCatalogue.Interfaces;

namespace GrobelnyKasprzak.MovieCatalogue.Services
{
    public class MovieService
    {
        private readonly IMovieRepository _repo;

        public MovieService()
        {
            ReflectionLoader loader = new();
            IStudioRepository studios = loader.GetRepository<IStudioRepository>();
            IDirectorRepository directors = loader.GetRepository<IDirectorRepository>();
            _repo = loader.GetRepository<IMovieRepository>(studios, directors);
        }

        public IEnumerable<IMovie> GetAllMovies() => _repo.GetAll();

        public IMovie? GetMovieById(int id) => _repo.GetById(id);

        public void AddMovie(IMovie movie) => _repo.Add(movie);

        public void UpdateMovie(IMovie movie) => _repo.Update(movie);

        public void DeleteMovie(int id) => _repo.Delete(id);
    }
}
