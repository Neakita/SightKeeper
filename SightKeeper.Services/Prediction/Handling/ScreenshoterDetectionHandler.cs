using System.Reactive.Linq;
using SightKeeper.Application.Prediction;
using SightKeeper.Domain.Model.DataSets;
using SightKeeper.Domain.Services;

namespace SightKeeper.Services.Prediction.Handling;

public sealed class ScreenshoterDetectionHandler : DetectionObserver
{
    public IObservable<float?> RequestedProbabilityThreshold => _parameters.ObservableMinimumProbability.Select(value => (float?)value);
    
    public ScreenshoterDetectionHandler(DataSet dataSet, DetectionScreenshotingParameters parameters, ScreenshotsDataAccess screenshotsDataAccess)
    {
        _dataSet = dataSet;
        _parameters = parameters;
        _screenshotsDataAccess = screenshotsDataAccess;
    }

    public void OnNext(DetectionData data)
    {
        if (ShouldMakeScreenshot(data))
            MakeScreenshot(data);
    }

    public void OnPaused()
    {
        _screenshotsDataAccess.SaveChanges(_dataSet.ScreenshotsLibrary);
    }

    private readonly DataSet _dataSet;
    private readonly DetectionScreenshotingParameters _parameters;
    private readonly ScreenshotsDataAccess _screenshotsDataAccess;
    private bool _isLoadingScreenshots;
    private DateTime _lastScreenshotTime = DateTime.UtcNow;
    
    private bool ShouldMakeScreenshot(DetectionData data)
    {
        return _parameters.MakeScreenshots &&
               !_isLoadingScreenshots &&
               IsDelayElapsed &&
               data.Items.Any(ShouldMakeScreenshot);
    }
    
    private async void MakeScreenshot(DetectionData data)
    {
        if (!await EnsureScreenshotLoaded())
            return;
        _lastScreenshotTime = DateTime.UtcNow;
        _dataSet.ScreenshotsLibrary.CreateScreenshot(data.Image);
    }

    private async Task<bool> EnsureScreenshotLoaded()
    {
        if (_screenshotsDataAccess.IsLoaded(_dataSet.ScreenshotsLibrary))
            return true;
        _isLoadingScreenshots = true;
        await _screenshotsDataAccess.Load(_dataSet.ScreenshotsLibrary);
        _isLoadingScreenshots = false;
        return false;
    }

    private bool IsDelayElapsed => _lastScreenshotTime + _parameters.ScreenshotingDelay <= DateTime.UtcNow;

    private bool ShouldMakeScreenshot(DetectionItem item)
    {
        return item.Probability >= _parameters.MinimumProbability && item.Probability <= _parameters.MaximumProbability;
    }
}