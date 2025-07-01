namespace CommandSample.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    public ReactiveUiCommandsViewModel ReactiveUiCommandsViewModel { get;  } = new ReactiveUiCommandsViewModel();
}