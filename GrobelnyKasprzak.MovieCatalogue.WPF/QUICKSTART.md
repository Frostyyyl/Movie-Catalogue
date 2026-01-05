# Quick Start Guide - Movie Catalogue WPF

## Understanding the Code Structure

### 1. The Window (MainWindow.xaml)
Think of this as your HTML - it describes what the user sees:
- Left panel: Movies (bigger, takes 2/3 of space)
- Right panel: Directors (smaller, takes 1/3 of space)

### 2. The Logic (ViewModels)
Think of this as your JavaScript - it handles user actions:
- **MainViewModel**: Container for everything
- **MoviesViewModel**: Handles movies (add, edit, delete, search)
- **DirectorsViewModel**: Handles directors (add, edit, delete, search)

### 3. The Connection (Data Binding)
WPF automatically connects UI to ViewModels using `{Binding}`:

```xaml
<!-- This textbox shows and edits MoviesVM.Title -->
<TextBox Text="{Binding MoviesVM.Title}" />

<!-- This button runs MoviesVM.AddCommand when clicked -->
<Button Command="{Binding MoviesVM.AddCommand}" />
```

## How Adding a Movie Works

### In the UI (MainWindow.xaml):
```xaml
<TextBox Text="{Binding MoviesVM.Title}" />
<Button Command="{Binding MoviesVM.AddCommand}" Content="Add" />
```

### In the ViewModel (MoviesViewModel.cs):
```csharp
// Property for the textbox
public string Title 
{ 
    get => _title;
    set 
    { 
        _title = value;
        OnPropertyChanged();  // Tell UI to update
        ValidateTitle();      // Check if valid
    }
}

// Command for the button
AddCommand = new RelayCommand(
    _ => AddMovie(),      // Run this when clicked
    _ => CanAddMovie()    // Button enabled when this returns true
);

// Method that runs when Add button is clicked
private void AddMovie()
{
    // 1. Create new movie
    var movie = _movieService.CreateNewMovie();
    
    // 2. Set properties from form
    movie.Title = Title;
    movie.Year = int.Parse(YearText);
    movie.Genre = Genre;
    movie.DirectorId = SelectedDirector.Id;
    
    // 3. Save to database
    _movieService.AddMovie(movie);
    
    // 4. Refresh the list
    LoadMovies();
    
    // 5. Clear form
    ClearForm();
    
    // 6. Show success message
    MessageBox.Show("Movie added successfully");
}

// Check if Add button should be enabled
private bool CanAddMovie()
{
    return !string.IsNullOrEmpty(Title) &&
           !string.IsNullOrEmpty(YearText) &&
           SelectedDirector != null &&
           string.IsNullOrEmpty(TitleError) &&
           string.IsNullOrEmpty(YearError);
}
```

## How Validation Works

### Step by step:
1. User types "T" in Title field
2. WPF calls `Title` setter with value "T"
3. Setter calls `ValidateTitle()`
4. `ValidateTitle()` sets `TitleError = "Title is required"` (too short)
5. `HasTitleError` becomes true
6. Error message appears in UI (bound to `TitleError`)
7. Red border appears (triggered by validation)
8. Add button disabled (`CanAddMovie()` returns false)

### When user types more:
1. User types more ? "The Matrix"
2. `ValidateTitle()` runs again
3. `TitleError = string.Empty` (valid now!)
4. `HasTitleError` becomes false
5. Error message disappears
6. Border returns to normal
7. Add button enabled (if other fields also valid)

## Key Properties Explained

### ObservableCollection
```csharp
private ObservableCollection<IMovie> _movies = new();
```
- Special list that automatically updates the UI when you add/remove items
- Like a regular List, but magical! ??

### INotifyPropertyChanged
```csharp
public string Title
{
    get => _title;
    set 
    { 
        _title = value;
        OnPropertyChanged(); // ? This is the magic!
    }
}
```
- `OnPropertyChanged()` tells WPF "Hey, this property changed, update the UI!"
- Without it, UI won't update when data changes

## Common Code Patterns

### Pattern 1: Property with Validation
```csharp
private string _title;
public string Title
{
    get => _title;
    set 
    { 
        _title = value;        // Store the value
        OnPropertyChanged();   // Update UI
        ValidateTitle();       // Check if valid
    }
}
```

### Pattern 2: Loading Data
```csharp
private void LoadMovies()
{
    _allMovies.Clear();                        // Clear old data
    foreach (var movie in _movieService.GetAllMovies())  // Get from database
        _allMovies.Add(movie);                 // Add to collection
    FilterMovies();                            // Apply filters
}
```

### Pattern 3: Filtering with LINQ
```csharp
var filtered = _allMovies.Where(m => 
    m.Title.Contains(SearchText, StringComparison.OrdinalIgnoreCase));
```
This is like SQL's `WHERE` - keeps only items matching the condition.

## Debugging Tips

### 1. Button not enabled?
- Check `CanAddMovie()` method
- Put breakpoint there
- See which condition returns false

### 2. UI not updating?
- Did you call `OnPropertyChanged()`?
- Is the property public?
- Is it using `{Binding}`?

### 3. Error message not showing?
- Check if `HasXxxError` is true
- Check if error `TextBlock` has correct `Visibility` binding
- Verify `Converter` is defined in resources

## XAML Shortcuts

```xaml
<!-- Basic binding (two-way) -->
Text="{Binding Title}"

<!-- Update immediately as user types -->
Text="{Binding Title, UpdateSourceTrigger=PropertyChanged}"

<!-- Command binding -->
Command="{Binding AddCommand}"

<!-- Show/hide based on boolean -->
Visibility="{Binding HasError, Converter={StaticResource BoolToVisibility}}"
```

## File Organization

```
ViewModels/
??? ViewModel.cs           ? Base class (simple!)
??? MainViewModel.cs       ? Container (very simple!)
??? MoviesViewModel.cs     ? Movies logic (main file)
??? DirectorsViewModel.cs  ? Directors logic (similar to Movies)

Commands/
??? RelayCommand.cs        ? Connects buttons to methods

MainWindow.xaml            ? UI design
MainWindow.xaml.cs         ? Empty (MVVM pattern!)
App.xaml                   ? Application settings
```

## Next Steps

1. Read `README.md` for detailed explanations
2. Look at `MoviesViewModel.cs` - understand one method at a time
3. Look at `MainWindow.xaml` - see how UI connects to ViewModel
4. Try modifying: add a new field, change validation rule, etc.
5. Use debugger: set breakpoints in `AddMovie()`, `CanAddMovie()`, etc.

## Remember

- **MVVM** = Model (data) + View (UI) + ViewModel (logic)
- **Binding** = automatic connection between UI and data
- **Commands** = connect buttons to methods
- **ObservableCollection** = list that updates UI automatically
- **OnPropertyChanged()** = tells UI to refresh

Good luck! ??
