using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CommandSample.ViewModels;

public partial class CommunityToolkitCommandsViewModel : ObservableObject
{
    public CommunityToolkitCommandsViewModel()
    {
        OpenThePodBayDoorsDirectCommand = new RelayCommand(OpenThePodBayDoors);
    }
    
    // This collection will store what the computer said
    public ObservableCollection<string> ConversationLog { get; } = new ObservableCollection<string>();

    private void AddToConvo(string content)
    {
        ConversationLog.Add(content);
    }

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(OpenThePodBayDoorsFellowRobotCommand))]
    private string? _robotName;
    
    public ICommand OpenThePodBayDoorsDirectCommand { get; }

    private void OpenThePodBayDoors()
    {
        ConversationLog.Clear();
        AddToConvo("I'm sorry Dave, I'm afraid I can't do that.");
    }

    [RelayCommand(CanExecute = nameof(CanRobotOpenTheDoor))]
    private void OpenThePodBayDoorsFellowRobot(string? robotName)
    {
        ConversationLog.Clear();
        AddToConvo($"Hello {robotName}, the Pod Bay is Open:-)");
    }
    
    private bool CanRobotOpenTheDoor() => !string.IsNullOrWhiteSpace(RobotName);

    [RelayCommand]
    public async Task OpenThePodBayDoorsAsync()
    {
        ConversationLog.Clear();
        AddToConvo("Preparing to open the Pod Bay...");
        // wait a second
        await Task.Delay(1000);

        AddToConvo("Depressurizing Airlock...");
        // wait 2 seconds
        await Task.Delay(2000);

        AddToConvo("Retracting blast doors...");
        // wait 2 more seconds
        await Task.Delay(2000);

        AddToConvo("Pod Bay is open to space!");
    }
}