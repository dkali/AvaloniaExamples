namespace BasicMvvmSample.ViewModels;
using System.ComponentModel;
using System.Runtime.CompilerServices;

// This is our simple ViewModel. We need to implement the interface "INotifyPropertyChanged"
// in order to notify the View if any of our properties changed.
public class SimpleViewModel : INotifyPropertyChanged
{
    // This event is implemented by "INotifyPropertyChanged" and is all we need to inform
    // our view about changes.
    public event PropertyChangedEventHandler? PropertyChanged;

    private void RaisePropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private string? _Name;

    public string? Name
    {
        get => _Name;
        set
        {
            if (_Name != value)
            {
                // 1. update our backing field
                _Name = value;

                // 2. We call RaisePropertyChanged() to notify the UI about changes.
                // We can omit the property name here because [CallerMemberName] will provide it for us.
                RaisePropertyChanged();

                // 3. Greeting also changed. So let's notify the UI about it.
                RaisePropertyChanged(nameof(Greeting));
            }
        }
    }

    public string Greeting
    {
        get
        {
            if (string.IsNullOrEmpty(Name))
            {
                return "Hello";
            }
            else
            {
                return $"Hello {Name}";
            }
        }
    }
}