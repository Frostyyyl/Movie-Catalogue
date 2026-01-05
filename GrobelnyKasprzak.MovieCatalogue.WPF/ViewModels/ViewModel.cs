using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace GrobelnyKasprzak.MovieCatalogue.WPF.ViewModels
{
    // Simple base class for ViewModels
    public abstract class ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
