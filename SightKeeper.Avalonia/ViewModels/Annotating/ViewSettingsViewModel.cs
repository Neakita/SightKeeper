using CommunityToolkit.Mvvm.Input;

namespace SightKeeper.Avalonia.ViewModels.Annotating;

internal sealed partial class ViewSettingsViewModel : ViewModel
{
    public bool ShowDetectorItems
    {
        get => _selectedScreenshotViewModel.ShowDetectorItems;
        set => SetProperty(
            _selectedScreenshotViewModel.ShowDetectorItems,
            value,
            newValue => _selectedScreenshotViewModel.ShowDetectorItems = newValue);
    }
    public bool ShowDetectedItems
    {
        get => _selectedScreenshotViewModel.ShowDetectedItems;
        set => SetProperty(
            _selectedScreenshotViewModel.ShowDetectedItems,
            value,
            newValue => _selectedScreenshotViewModel.ShowDetectedItems = newValue);
    }
    
    public ViewSettingsViewModel(SelectedScreenshotViewModel selectedScreenshotViewModel)
    {
        _selectedScreenshotViewModel = selectedScreenshotViewModel;
    }

    private readonly SelectedScreenshotViewModel _selectedScreenshotViewModel;

    [RelayCommand]
    private void ToggleBoth()
    {
        ShowDetectorItems = !ShowDetectorItems;
        ShowDetectedItems = !ShowDetectedItems;
    }
}