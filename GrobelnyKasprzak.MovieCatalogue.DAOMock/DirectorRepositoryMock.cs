using GrobelnyKasprzak.MovieCatalogue.DAOMock.Models;
using GrobelnyKasprzak.MovieCatalogue.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace GrobelnyKasprzak.MovieCatalogue.DAOMock
{
    public class DirectorRepositoryMock : IDirectorRepository
    {
        private static readonly List<Director> _directors =
        [
            new Director { Id = 1, Name = "Lana & Lilly Wachowski", BirthYear = 1988 },
            new Director { Id = 2, Name = "Andrew Adamson", BirthYear = 1987 },
            new Director { Id = 3, Name = "Quentin Tarantino", BirthYear = 1991 },
            new Director { Id = 4, Name = "Christopher Nolan", BirthYear = 1972 }
        ];

        private static int _nextId = 5;

        public IEnumerable<IDirector> GetAll()
        {
            return _directors;
        }

        public IDirector? GetById(int id)
        {
            return _directors.FirstOrDefault(d => d.Id == id);
        }

        public IDirector CreateNew()
        {
            return new Director { BirthYear = DateTime.Now.Year };
        }

        public void Add(IDirector director)
        {
            var newDirector = new Director
            {
                Id = _nextId++,
                Name = director.Name,
                BirthYear = director.BirthYear
            };

            ValidateDirector(newDirector);

            _directors.Add(newDirector);
        }


        public void Update(IDirector director)
        {
            var existing = GetById(director.Id)
                ?? throw new KeyNotFoundException($"Director with ID {director.Id} not found.");

            ValidateDirector(director);

            existing.Name = director.Name;
            existing.BirthYear = director.BirthYear;
        }

        public void Delete(int id)
        {
            var director = GetById(id)
                ?? throw new KeyNotFoundException($"Director with ID {id} not found.");

            _directors.Remove((Director)director);
        }

        public bool Exists(string? name = null, int? birthYear = null)
        {
            return _directors.Any(m =>
                (name == null || m.Name == name) &&
                (birthYear == null || m.BirthYear == birthYear)
            );
        }

        private static void ValidateDirector(IDirector director)
        {
            var context = new ValidationContext(director);
            Validator.ValidateObject(director, context, validateAllProperties: true);
        }
    }
}
