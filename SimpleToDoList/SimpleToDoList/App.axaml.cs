using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Markup.Xaml;
using SimpleToDoList.Services;
using SimpleToDoList.ViewModels;
using SimpleToDoList.Views;

namespace SimpleToDoList;

public partial class App : Application
{
    private readonly MainWindowViewModel _mainWindowViewModel = new MainWindowViewModel();
    
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override async void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // Avoid duplicate validations from both Avalonia and the CommunityToolkit. 
            // More info: https://docs.avaloniaui.net/docs/guides/development-guides/data-validation#manage-validationplugins
            DisableAvaloniaDataAnnotationValidation();
            desktop.MainWindow = new MainWindow
            {
                DataContext = _mainWindowViewModel
            };
            
            // Listen to the ShutdownRequested-event
            desktop.ShutdownRequested += DesktopOnShutdownRequested;
        }

        await InitMainViewModelAsync();
        
        base.OnFrameworkInitializationCompleted();
    }

    private void DisableAvaloniaDataAnnotationValidation()
    {
        // Get an array of plugins to remove
        var dataValidationPluginsToRemove =
            BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

        // remove each entry found
        foreach (var plugin in dataValidationPluginsToRemove)
        {
            BindingPlugins.DataValidators.Remove(plugin);
        }
    }

    private bool _canClose;

    private async void DesktopOnShutdownRequested(object? sender, ShutdownRequestedEventArgs e)
    {
        e.Cancel = !_canClose;  // cancel closing event first time

        if (!_canClose)
        {
            // To save the items, we map them to the ToDoItem-Model which is better suited for I/O operations
            var itemsToSave = _mainWindowViewModel.ToDoItems.Select(item => item.GetToDoItem());
            await ToDoListFileService.SaveToFileAsync(itemsToSave);
            
            // Set _canClose to true and Close this Window again
            _canClose = true;
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.Shutdown();
            }
        }
    }

    private async Task InitMainViewModelAsync()
    {
        var itemsToLoad = await ToDoListFileService.LoadFromFileAsync();

        if (itemsToLoad is not null)
        {
            foreach (var item in itemsToLoad)
            {
                _mainWindowViewModel.ToDoItems.Add(new ToDoItemViewModel(item));
            }
        }
    }
}