using GrobelnyKasprzak.MovieCatalogue.Services;

namespace GrobelnyKasprzak.MovieCatalogue.WPF.ViewModels
{
    /// <summary>
    /// Main ViewModel that connects Movies and Directors sections.
    /// This is the top-level ViewModel for the entire window.
    /// </summary>
    public class MainViewModel : ViewModel
    {
        /// <summary>
        /// ViewModel for the Movies section (left side of window).
        /// </summary>
        public MoviesViewModel MoviesVM { get; }

        /// <summary>
        /// ViewModel for the Directors section (right side of window).
        /// </summary>
        public DirectorsViewModel DirectorsVM { get; }

        /// <summary>
        /// Creates the main ViewModel and initializes both sub-ViewModels.
        /// </summary>
        public MainViewModel()
        {
            // Create service instances (handles database operations)
            var movieService = new MovieService();
            var directorService = new DirectorService();

            // Create ViewModels for each section
            MoviesVM = new MoviesViewModel(movieService, directorService);
            DirectorsVM = new DirectorsViewModel(directorService, movieService);
        }
    }
}
