using GrobelnyKasprzak.MovieCatalogue.Core;
using GrobelnyKasprzak.MovieCatalogue.Interfaces;
using GrobelnyKasprzak.MovieCatalogue.Services;
using System.Collections.ObjectModel;
using System.Windows;
using GrobelnyKasprzak.MovieCatalogue.WPF.Commands;

namespace GrobelnyKasprzak.MovieCatalogue.WPF.ViewModels
{
    public class MoviesViewModel : ViewModel
    {
        private readonly MovieService _movieService;
        private readonly DirectorService _directorService;

        // Fields
        private ObservableCollection<IMovie> _movies = new();
        private ObservableCollection<IMovie> _allMovies = new();

        private IMovie? _selectedMovie;
        private string _searchText = string.Empty;
        private string _selectedGenreFilter = "All";

        private string _title = string.Empty;
        private string _yearText = string.Empty;
        private MovieGenre _genre = MovieGenre.Action;
        private IDirector? _selectedDirector;

        private string _titleError = string.Empty;
        private string _yearError = string.Empty;
        private string _directorError = string.Empty;

        // Constructor
        public MoviesViewModel(MovieService movieService, DirectorService directorService)
        {
            _movieService = movieService;
            _directorService = directorService;

            AddCommand = new RelayCommand(_ => AddMovie(), _ => CanAddMovie());
            UpdateCommand = new RelayCommand(_ => UpdateMovie(), _ => CanUpdateMovie());
            DeleteCommand = new RelayCommand(_ => DeleteMovie(), _ => CanDeleteMovie());

            LoadMovies();
        }

        // Properties for data binding
        public ObservableCollection<IMovie> Movies
        {
            get => _movies;
            set { _movies = value; OnPropertyChanged(); }
        }

        public ObservableCollection<IDirector> Directors
        {
            get
            {
                var directors = new ObservableCollection<IDirector>();
                foreach (var d in _directorService.GetAllDirectors())
                    directors.Add(d);
                return directors;
            }
        }

        public IMovie? SelectedMovie
        {
            get => _selectedMovie;
            set
            {
                _selectedMovie = value;
                OnPropertyChanged();
                
                if (value != null)
                {
                    Title = value.Title;
                    YearText = value.Year.ToString();
                    Genre = value.Genre;
                    SelectedDirector = Directors.FirstOrDefault(d => d.Id == value.DirectorId);
                }
                else
                {
                    ClearForm();
                }
            }
        }

        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged();
                FilterMovies();
            }
        }

        public string SelectedGenreFilter
        {
            get => _selectedGenreFilter;
            set
            {
                _selectedGenreFilter = value;
                OnPropertyChanged();
                FilterMovies();
            }
        }

        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged();
                ValidateTitle();
            }
        }

        public string YearText
        {
            get => _yearText;
            set
            {
                // Only allow numbers
                if (string.IsNullOrEmpty(value) || int.TryParse(value, out _))
                {
                    _yearText = value;
                    OnPropertyChanged();
                    ValidateYear();
                }
            }
        }

        public MovieGenre Genre
        {
            get => _genre;
            set { _genre = value; OnPropertyChanged(); }
        }

        public IDirector? SelectedDirector
        {
            get => _selectedDirector;
            set
            {
                _selectedDirector = value;
                OnPropertyChanged();
                ValidateDirector();
            }
        }

        // Error properties
        public string TitleError
        {
            get => _titleError;
            set { _titleError = value; OnPropertyChanged(); OnPropertyChanged(nameof(HasTitleError)); }
        }

        public string YearError
        {
            get => _yearError;
            set { _yearError = value; OnPropertyChanged(); OnPropertyChanged(nameof(HasYearError)); }
        }

        public string DirectorError
        {
            get => _directorError;
            set { _directorError = value; OnPropertyChanged(); OnPropertyChanged(nameof(HasDirectorError)); }
        }

        public bool HasTitleError => !string.IsNullOrEmpty(TitleError);
        public bool HasYearError => !string.IsNullOrEmpty(YearError);
        public bool HasDirectorError => !string.IsNullOrEmpty(DirectorError);

        // Lists for ComboBoxes
        public Array Genres => Enum.GetValues(typeof(MovieGenre));

        public ObservableCollection<string> GenresWithAll
        {
            get
            {
                var list = new ObservableCollection<string> { "All" };
                foreach (var genre in Enum.GetNames(typeof(MovieGenre)))
                    list.Add(genre);
                return list;
            }
        }

        // Commands
        public RelayCommand AddCommand { get; }
        public RelayCommand UpdateCommand { get; }
        public RelayCommand DeleteCommand { get; }

        // Validation methods
        private void ValidateTitle()
        {
            TitleError = string.IsNullOrWhiteSpace(Title) ? "Title is required" : string.Empty;
        }

        private void ValidateYear()
        {
            if (string.IsNullOrWhiteSpace(YearText))
            {
                YearError = "Year is required";
                return;
            }

            if (!int.TryParse(YearText, out int year))
            {
                YearError = "Invalid year";
                return;
            }

            if (year < 1895)
            {
                YearError = "Year must be >= 1895";
                return;
            }

            if (year > DateTime.Now.Year + 10)
            {
                YearError = $"Year must be <= {DateTime.Now.Year + 10}";
                return;
            }

            YearError = string.Empty;
        }

        private void ValidateDirector()
        {
            DirectorError = SelectedDirector == null ? "Director is required" : string.Empty;
        }

        // Data operations
        private void LoadMovies()
        {
            _allMovies.Clear();
            foreach (var movie in _movieService.GetAllMovies())
                _allMovies.Add(movie);
            FilterMovies();
        }

        private void FilterMovies()
        {
            Movies.Clear();
            var filtered = _allMovies.AsEnumerable();

            // Filter by search text
            if (!string.IsNullOrWhiteSpace(SearchText))
                filtered = filtered.Where(m => m.Title.Contains(SearchText, StringComparison.OrdinalIgnoreCase));

            // Filter by genre
            if (SelectedGenreFilter != "All" && Enum.TryParse<MovieGenre>(SelectedGenreFilter, out var genreFilter))
                filtered = filtered.Where(m => m.Genre == genreFilter);

            foreach (var movie in filtered)
                Movies.Add(movie);
        }

        private bool CanAddMovie()
        {
            return string.IsNullOrEmpty(TitleError) &&
                   string.IsNullOrEmpty(YearError) &&
                   string.IsNullOrEmpty(DirectorError) &&
                   !string.IsNullOrWhiteSpace(Title) &&
                   !string.IsNullOrWhiteSpace(YearText) &&
                   SelectedDirector != null;
        }

        private void AddMovie()
        {
            try
            {
                var movie = _movieService.CreateNewMovie();
                movie.Title = Title;
                movie.Year = int.Parse(YearText);
                movie.Genre = Genre;
                movie.DirectorId = SelectedDirector!.Id;

                _movieService.AddMovie(movie);
                LoadMovies();
                ClearForm();
                MessageBox.Show("Movie added successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool CanUpdateMovie()
        {
            return SelectedMovie != null && CanAddMovie();
        }

        private void UpdateMovie()
        {
            try
            {
                if (SelectedMovie == null) return;

                SelectedMovie.Title = Title;
                SelectedMovie.Year = int.Parse(YearText);
                SelectedMovie.Genre = Genre;
                SelectedMovie.DirectorId = SelectedDirector!.Id;

                _movieService.UpdateMovie(SelectedMovie);
                LoadMovies();
                ClearForm();
                MessageBox.Show("Movie updated successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool CanDeleteMovie() => SelectedMovie != null;

        private void DeleteMovie()
        {
            try
            {
                if (SelectedMovie == null) return;

                var result = MessageBox.Show(
                    $"Delete '{SelectedMovie.Title}'?",
                    "Confirm Delete",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    _movieService.DeleteMovie(SelectedMovie.Id);
                    LoadMovies();
                    ClearForm();
                    MessageBox.Show("Movie deleted successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ClearForm()
        {
            Title = string.Empty;
            YearText = string.Empty;
            Genre = MovieGenre.Action;
            SelectedDirector = null;
            SelectedMovie = null;
        }
    }
}
