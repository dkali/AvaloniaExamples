using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using ReactiveUI;

namespace CommandSample.ViewModels;

public class ReactiveUiCommandsViewModel : ReactiveObject
{
    public ReactiveUiCommandsViewModel()
    {
        // The IObservable<bool> is needed to enable or disable the command depending on valid parameters
        // The Observable listens to RobotName and will enable the Command if the name is not empty.
        IObservable<bool> canExecuteFellowRobotCommand =
            this.WhenAnyValue(vm => vm.RobotName, (name) => !string.IsNullOrEmpty(name));
        
        OpenThePodBayDoorsDirectCommand = ReactiveCommand.Create(OpenThePodBayDoors);
        OpenThePodBayDoorsFellowRobotCommand =
            ReactiveCommand.Create<string?>(name => OpenThePodBayDoorsFellowRobot(name), canExecuteFellowRobotCommand);
        OpenThePodBayDoorsAsyncCommand = ReactiveCommand.CreateFromTask(OpenThePodBayDoorsAsync);
    }
    
    public ObservableCollection<string> ConversationLog { get; } = new ObservableCollection<string>();

    private void AddToConvo(string content)
    {
        ConversationLog.Add(content);
    }

    private string? _RobotName;

    public string? RobotName
    {
        get => _RobotName;
        set => this.RaiseAndSetIfChanged(ref _RobotName, value);
    }
    
    public ICommand OpenThePodBayDoorsDirectCommand { get;  }
    public ICommand OpenThePodBayDoorsFellowRobotCommand { get;  }
    public ICommand OpenThePodBayDoorsAsyncCommand { get; }
    
    // The method that will be executed when the command is invoked
    private void OpenThePodBayDoors()
    {
        ConversationLog.Clear();
        AddToConvo("I'm sorry, Dave, I'm afraid I can't do that.");
    }

    private void OpenThePodBayDoorsFellowRobot(string? robotName)
    {
        ConversationLog.Clear();
        AddToConvo($"Hello {robotName}, the Pod Bay is open :-)");
    }

    private async Task OpenThePodBayDoorsAsync()
    {
        ConversationLog.Clear();
        AddToConvo(("Preparing to open the Pod Bay..."));
        await Task.Delay(1000);
        
        AddToConvo(("Depressurizing Airlock..."));
        await Task.Delay(2000);
        
        AddToConvo("Retracting blast doors...");
        await Task.Delay(2000);
        
        AddToConvo("Pod Bay is open to space!");
    }
}