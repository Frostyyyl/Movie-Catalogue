# Movie Catalogue WPF Application

## Simple Code Guide for Beginners

This WPF application allows you to manage movies and directors using a simple interface.

### Project Structure

```
WPF/
??? ViewModels/           # Logic for handling data
?   ??? ViewModel.cs      # Base class (handles property changes)
?   ??? MainViewModel.cs  # Main window logic
?   ??? MoviesViewModel.cs     # Movies logic
?   ??? DirectorsViewModel.cs  # Directors logic
??? Commands/
?   ??? RelayCommand.cs   # Command pattern for buttons
??? MainWindow.xaml       # UI design (what you see)
??? App.xaml              # Application entry point
```

### How It Works

#### 1. **ViewModel.cs** (Base Class)
- Implements `INotifyPropertyChanged` - tells the UI when data changes
- `OnPropertyChanged()` - method to notify UI about updates

#### 2. **MoviesViewModel.cs**
Key properties:
- `Movies` - list of all movies displayed in the grid
- `SelectedMovie` - currently selected movie
- `Title`, `YearText`, `Genre`, `SelectedDirector` - form fields
- `TitleError`, `YearError`, `DirectorError` - validation messages

Key methods:
- `LoadMovies()` - loads movies from database
- `FilterMovies()` - filters by search text and genre
- `AddMovie()` - creates new movie
- `UpdateMovie()` - updates existing movie
- `DeleteMovie()` - removes movie
- `ValidateTitle()`, `ValidateYear()`, `ValidateDirector()` - check if input is valid

#### 3. **DirectorsViewModel.cs**
Similar to MoviesViewModel but simpler:
- `Directors` - list of directors
- `Name` - director name field
- `AddDirector()`, `UpdateDirector()`, `DeleteDirector()` - CRUD operations

#### 4. **MainWindow.xaml** (User Interface)
Two main sections:
- **Left side (2/3 width)**: Movies management
  - Search box and genre filter
  - Form: Title, Year, Genre, Director
  - Buttons: Add, Update, Delete
  - Data grid showing all movies
  
- **Right side (1/3 width)**: Directors management
  - Search box
  - Form: Name
  - Buttons: Add, Update, Delete
  - Data grid showing all directors

### Data Binding

The UI connects to ViewModels using `{Binding}`:

```xaml
<!-- Two-way binding: UI ? ViewModel -->
Text="{Binding MoviesVM.Title, UpdateSourceTrigger=PropertyChanged}"

<!-- One-way binding: ViewModel ? UI -->
ItemsSource="{Binding MoviesVM.Movies}"

<!-- Command binding: Button ? Method -->
Command="{Binding MoviesVM.AddCommand}"
```

### Validation Flow

1. User types in a field (e.g., Title)
2. `Title` property setter is called
3. `ValidateTitle()` runs automatically
4. If invalid: `TitleError` gets error message
5. `HasTitleError` becomes true
6. Error message appears below the field (red text)
7. Add/Update buttons become disabled until all fields are valid

### Commands (Buttons)

Each button uses `RelayCommand`:
```csharp
AddCommand = new RelayCommand(
    _ => AddMovie(),      // What to do when clicked
    _ => CanAddMovie()    // When button should be enabled
);
```

### Adding a New Movie - Step by Step

1. User fills in: Title, Year, Genre, Director
2. Each field validates automatically
3. If all valid: Add button becomes enabled
4. User clicks Add
5. `AddMovie()` method runs:
   - Creates new movie object
   - Sets properties from form
   - Calls `_movieService.AddMovie()`
   - Refreshes the list
   - Clears the form
   - Shows success message

### Common Patterns

**ObservableCollection**: Automatically updates UI when items added/removed
```csharp
private ObservableCollection<IMovie> _movies = new();
```

**Property with notification**:
```csharp
private string _title;
public string Title
{
    get => _title;
    set 
    { 
        _title = value;
        OnPropertyChanged();  // Tells UI to update
    }
}
```

**LINQ filtering**:
```csharp
var filtered = _allMovies.Where(m => 
    m.Title.Contains(SearchText, StringComparison.OrdinalIgnoreCase));
```

### Styling

Styles are defined in `Window.Resources`:
- `ButtonStyle` - blue buttons
- `DeleteButtonStyle` - red delete button
- `TextBoxStyle` - input fields
- `ComboBoxStyle` - dropdown lists
- `ErrorTextStyle` - red error messages
- `LabelStyle` - field labels

### Error Messages

All messages are in English:
- "Title is required"
- "Year must be >= 1895"
- "Director is required"
- "Movie added successfully"
- "Cannot delete director with movies"

### Tips for Understanding the Code

1. **Start with MainWindow.xaml** - see the visual layout
2. **Follow data bindings** - trace from UI to ViewModel
3. **Read one ViewModel at a time** - they're independent
4. **Understand properties first** - they hold the data
5. **Then understand methods** - they perform actions
6. **Commands connect UI to logic** - buttons ? methods

### Key Concepts

- **MVVM Pattern**: Model-View-ViewModel separates UI from logic
- **Data Binding**: Automatic sync between UI and data
- **Commands**: Connect button clicks to methods
- **ObservableCollection**: List that notifies UI of changes
- **INotifyPropertyChanged**: Interface for property change notifications
