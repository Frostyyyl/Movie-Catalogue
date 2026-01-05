using GrobelnyKasprzak.MovieCatalogue.Interfaces;
using GrobelnyKasprzak.MovieCatalogue.Services;
using System.Collections.ObjectModel;
using System.Windows;
using GrobelnyKasprzak.MovieCatalogue.WPF.Commands;

namespace GrobelnyKasprzak.MovieCatalogue.WPF.ViewModels
{
    public class DirectorsViewModel : ViewModel
    {
        private readonly DirectorService _directorService;
        private readonly MovieService _movieService;

        private ObservableCollection<IDirector> _directors = new();
        private ObservableCollection<IDirector> _allDirectors = new();
        
        private IDirector? _selectedDirector;
        private string _searchText = string.Empty;
        private string _name = string.Empty;
        private string _nameError = string.Empty;

        public DirectorsViewModel(DirectorService directorService, MovieService movieService)
        {
            _directorService = directorService;
            _movieService = movieService;

            AddCommand = new RelayCommand(_ => AddDirector(), _ => CanAdd());
            UpdateCommand = new RelayCommand(_ => UpdateDirector(), _ => CanUpdate());
            DeleteCommand = new RelayCommand(_ => DeleteDirector(), _ => CanDelete());

            LoadDirectors();
        }

        // Properties for data binding
        public ObservableCollection<IDirector> Directors
        {
            get => _directors;
            set { _directors = value; OnPropertyChanged(); }
        }

        public IDirector? SelectedDirector
        {
            get => _selectedDirector;
            set
            {
                _selectedDirector = value;
                OnPropertyChanged();
                
                if (value != null)
                    Name = value.Name;
                else
                    Name = string.Empty;
            }
        }

        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged();
                FilterDirectors();
            }
        }

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
                ValidateName();
            }
        }

        // Error properties
        public string NameError
        {
            get => _nameError;
            set { _nameError = value; OnPropertyChanged(); OnPropertyChanged(nameof(HasNameError)); }
        }

        public bool HasNameError => !string.IsNullOrEmpty(NameError);

        // Commands
        public RelayCommand AddCommand { get; }
        public RelayCommand UpdateCommand { get; }
        public RelayCommand DeleteCommand { get; }

        // Validation
        private void ValidateName()
        {
            NameError = string.IsNullOrWhiteSpace(Name) ? "Name is required" : string.Empty;
        }

        private bool CanAdd()
        {
            return string.IsNullOrEmpty(NameError) && !string.IsNullOrWhiteSpace(Name);
        }

        private bool CanUpdate()
        {
            return SelectedDirector != null && CanAdd();
        }

        private bool CanDelete()
        {
            return SelectedDirector != null;
        }

        // Data operations
        public void LoadDirectors()
        {
            _allDirectors.Clear();
            foreach (var d in _directorService.GetAllDirectors())
                _allDirectors.Add(d);
            FilterDirectors();
        }

        private void FilterDirectors()
        {
            Directors.Clear();
            
            var filtered = string.IsNullOrWhiteSpace(SearchText)
                ? _allDirectors
                : _allDirectors.Where(d => d.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase));
            
            foreach (var d in filtered)
                Directors.Add(d);
        }

        private void AddDirector()
        {
            try
            {
                var director = _directorService.CreateNewDirector();
                director.Name = Name;
                _directorService.AddDirector(director);
                
                LoadDirectors();
                SelectedDirector = null;
                MessageBox.Show("Director added successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UpdateDirector()
        {
            try
            {
                if (SelectedDirector == null) return;
                
                SelectedDirector.Name = Name;
                _directorService.UpdateDirector(SelectedDirector);
                
                LoadDirectors();
                SelectedDirector = null;
                MessageBox.Show("Director updated successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DeleteDirector()
        {
            try
            {
                if (SelectedDirector == null) return;
                
                // Check if director has movies
                var hasMovies = _movieService.GetAllMovies().Any(m => m.DirectorId == SelectedDirector.Id);
                if (hasMovies)
                {
                    MessageBox.Show("Cannot delete director with movies", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var result = MessageBox.Show(
                    $"Delete '{SelectedDirector.Name}'?",
                    "Confirm Delete",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    _directorService.DeleteDirector(SelectedDirector.Id);
                    LoadDirectors();
                    SelectedDirector = null;
                    MessageBox.Show("Director deleted successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
