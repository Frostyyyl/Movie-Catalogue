using AutoMapper;
using GrobelnyKasprzak.MovieCatalogue.Core;
using GrobelnyKasprzak.MovieCatalogue.MVC.Mappings;
using GrobelnyKasprzak.MovieCatalogue.MVC.Models.Dto;
using GrobelnyKasprzak.MovieCatalogue.MVC.ViewModels;
using GrobelnyKasprzak.MovieCatalogue.Services;
using Microsoft.AspNetCore.Mvc;

namespace GrobelnyKasprzak.MovieCatalogue.MVC.Controllers
{
    public class MoviesController : Controller
    {
        private readonly ILogger<MoviesController> _logger;
        private readonly IMapper _mapper;
        private readonly MovieService _movieService = new();
        private readonly DirectorService _directorService = new();

        public MoviesController(ILogger<MoviesController> logger, IMapper mapper)
        {
            _logger = logger;
            _mapper = mapper;
        }

        // GET: MoviesController
        public ActionResult Index(string? search)
        {
            var movies = _movieService.GetAllMovies();

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim().ToLower();

                movies = [.. movies.Where(m =>
                    m.Title.Contains(search, StringComparison.CurrentCultureIgnoreCase))];
            }

            var viewModel = new List<MovieViewModel>();

            foreach (var movie in movies)
            {
                var director = _directorService.GetDirectorById(movie.DirectorId);

                viewModel.Add(_mapper.Map<MovieViewModel>(movie, opt =>
                {
                    opt.Items[MappingKeys.DirectorName] = director?.Name;
                }));
            }

            @ViewData["Search"] = search;

            return View(viewModel);
        }

        // GET: MoviesController/Details/5
        public ActionResult Details(int id)
        {
            var movie = _movieService.GetMovieById(id);
            if (movie == null) return NotFound();

            var director = _directorService.GetDirectorById(movie.DirectorId);

            var viewModel = _mapper.Map<MovieViewModel>(movie, opt =>
            {
                opt.Items[MappingKeys.DirectorName] = director?.Name;
            });

            return View(viewModel);
        }

        // GET: MoviesController/Create
        public ActionResult Create()
        {
            var newMovie = _movieService.CreateNewMovie();

            var director = _directorService.GetDirectorById(newMovie.DirectorId);
            var availableDirectors = _directorService.GetAllDirectors();
            var availableGenres = Enum.GetValues<MovieGenre>();

            var viewModel = _mapper.Map<MovieViewModel>(newMovie, opt =>
            {
                opt.Items[MappingKeys.DirectorName] = director?.Name;
                opt.Items[MappingKeys.AvailableDirectors] = availableDirectors;
                opt.Items[MappingKeys.AvailableGenres] = availableGenres;
            });

            return View(viewModel);
        }

        // POST: MoviesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(MovieViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var directors = _directorService.GetAllDirectors();
                var genres = Enum.GetValues<MovieGenre>();

                _mapper.Map(model, model, opt =>
                {
                    opt.Items[MappingKeys.AvailableDirectors] = directors;
                    opt.Items[MappingKeys.AvailableGenres] = genres;
                });

                return View(model);
            }

            var movie = _mapper.Map<MovieDto>(model);
            _movieService.AddMovie(movie);

            return RedirectToAction(nameof(Index));
        }

        // GET: MoviesController/Edit/5
        public ActionResult Edit(int id)
        {
            var movie = _movieService.GetMovieById(id);
            if (movie == null) return NotFound();

            var director = _directorService.GetDirectorById(movie.DirectorId);
            var availableDirectors = _directorService.GetAllDirectors();
            var availableGenres = Enum.GetValues<MovieGenre>();

            var viewModel = _mapper.Map<MovieViewModel>(movie, opt =>
            {
                opt.Items[MappingKeys.DirectorName] = director?.Name;
                opt.Items[MappingKeys.AvailableDirectors] = availableDirectors;
                opt.Items[MappingKeys.AvailableGenres] = availableGenres;
            });

            return View(viewModel);
        }

        // POST: MoviesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, MovieViewModel viewModel)
        {
            if (id != viewModel.Id) return BadRequest();

            if (!ModelState.IsValid)
            {
                var directors = _directorService.GetAllDirectors();
                var genres = Enum.GetValues<MovieGenre>();

                _mapper.Map(viewModel, viewModel, opt =>
                {
                    opt.Items[MappingKeys.AvailableDirectors] = directors;
                    opt.Items[MappingKeys.AvailableGenres] = genres;
                });

                return View(viewModel);
            }

            var movieToUpdate = _mapper.Map<MovieDto>(viewModel);
            _movieService.UpdateMovie(movieToUpdate);

            return RedirectToAction(nameof(Details), new { id });
        }


        //GET: MoviesController/Delete/5
        public ActionResult Delete(int id)
        {
            var movie = _movieService.GetMovieById(id);
            if (movie == null) return NotFound();

            return View(movie);
        }

        // POST: MoviesController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, MovieViewModel model)
        {
            _movieService.DeleteMovie(id);

            return RedirectToAction(nameof(Index));
        }
    }
}
