using System;
using CommunityToolkit.Diagnostics;
using CommunityToolkit.Mvvm.Input;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.Detector;

namespace SightKeeper.Avalonia.ViewModels.Annotating.AnnotatorTools;

public sealed partial class DetectorAnnotatorToolsViewModel : ViewModel, AnnotatorTools<DetectorModel>, IDisposable
{
    public DetectorAnnotatorToolsViewModel(DetectorModel model, AnnotatorScreenshotsViewModel screenshotsViewModel)
    {
        _model = model;
        _screenshotsViewModel = screenshotsViewModel;
        _disposable = _screenshotsViewModel.SelectedScreenshotChanged.Subscribe(OnScreenshotSelected);
    }
    
    [RelayCommand(CanExecute = nameof(CanMarkSelectedScreenshotAsAsset))]
    private void MarkSelectedScreenshotAsAsset()
    {
        var screenshotViewModel = _screenshotsViewModel.SelectedScreenshot;
        Guard.IsNotNull(screenshotViewModel);
        _model.MakeAsset(screenshotViewModel.Screenshot);
        screenshotViewModel.NotifyIsAssetChanged();
    }

    private bool CanMarkSelectedScreenshotAsAsset() =>
        _screenshotsViewModel.SelectedScreenshot?.IsAsset == false;

    [RelayCommand(CanExecute = nameof(CanUnMarkSelectedScreenshotAsAsset))]
    private void UnMarkSelectedScreenshotAsAsset()
    {
        var screenshotViewModel = _screenshotsViewModel.SelectedScreenshot;
        Guard.IsNotNull(screenshotViewModel);
        var detectorAsset = screenshotViewModel.Screenshot.GetAsset<DetectorAsset>();
        _model.DeleteAsset(detectorAsset);
        screenshotViewModel.NotifyIsAssetChanged();
    }

    private bool CanUnMarkSelectedScreenshotAsAsset() =>
        _screenshotsViewModel.SelectedScreenshot?.IsAsset == true;

    private readonly DetectorModel _model;
    private readonly AnnotatorScreenshotsViewModel _screenshotsViewModel;
    private readonly IDisposable _disposable;

    private IDisposable? _selectedScreenshotDisposable;

    private void OnScreenshotSelected(ScreenshotViewModel? screenshot)
    {
        _selectedScreenshotDisposable?.Dispose();
        MarkSelectedScreenshotAsAssetCommand.NotifyCanExecuteChanged();
        UnMarkSelectedScreenshotAsAssetCommand.NotifyCanExecuteChanged();
        if (screenshot == null)
            return;
        _selectedScreenshotDisposable = screenshot.IsAssetChanged.Subscribe(_ =>
        {
            MarkSelectedScreenshotAsAssetCommand.NotifyCanExecuteChanged();
            UnMarkSelectedScreenshotAsAssetCommand.NotifyCanExecuteChanged();
        });
    }

    public void Dispose()
    {
        _disposable.Dispose();
        _selectedScreenshotDisposable?.Dispose();
    }
}