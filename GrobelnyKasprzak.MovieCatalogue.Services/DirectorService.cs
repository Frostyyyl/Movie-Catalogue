using GrobelnyKasprzak.MovieCatalogue.Interfaces;

namespace GrobelnyKasprzak.MovieCatalogue.Services
{
    public class DirectorService(IDirectorRepository directorRepository, IMovieRepository movieRepository) : IDirectorService
    {
        private readonly IDirectorRepository _directorRepository = directorRepository;
        private readonly IMovieRepository _movieRepository = movieRepository;

        public IEnumerable<IDirector> GetAllDirectors() => _directorRepository.GetAll();

        public IDirector? GetDirectorById(int id) => _directorRepository.GetById(id);

        public void AddDirector(IDirector director)
        {
            if (_directorRepository.Exists(name: director.Name, birthYear: director.BirthYear))
            {
                throw new InvalidOperationException("This director is already in the system.");
            }

            _directorRepository.Add(director);
        }
        public void UpdateDirector(IDirector director)
        {
            if (_directorRepository.Exists(name: director.Name, birthYear: director.BirthYear))
            {
                throw new InvalidOperationException("This director is already in the system.");
            }

            _directorRepository.Update(director);
        }
        public void DeleteDirector(int id)
        {
            var hasMovies = _movieRepository.GetByDirectorId(id).Any();

            if (hasMovies)
            {
                throw new InvalidOperationException("Cannot delete a director who has movies assigned.");
            }

            _directorRepository.Delete(id);
        }

        public IDirector CreateNewDirector() => _directorRepository.CreateNew();
    }
}
