namespace CommandSample.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    public ReactiveUiCommandsViewModel ReactiveUiCommandsViewModel { get;  } = new ReactiveUiCommandsViewModel();
    public CommunityToolkitCommandsViewModel CommunityToolkitCommandsViewModel { get;  } = new CommunityToolkitCommandsViewModel();
}