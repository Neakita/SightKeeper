using System.Reactive.Linq;
using SightKeeper.Domain.Model.DataSets;
using SightKeeper.Domain.Services;

namespace SightKeeper.Application.Prediction.Handling;

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

    private readonly DataSet _dataSet;
    private readonly DetectionScreenshotingParameters _parameters;
    private readonly ScreenshotsDataAccess _screenshotsDataAccess;
    private DateTime _lastScreenshotTime = DateTime.UtcNow;
    
    private bool ShouldMakeScreenshot(DetectionData data)
    {
        return _parameters.MakeScreenshots &&
               IsDelayElapsed &&
               data.Items.Any(ShouldMakeScreenshot);
    }
    
    private void MakeScreenshot(DetectionData data)
    {
        _lastScreenshotTime = DateTime.UtcNow;
        _screenshotsDataAccess.CreateScreenshot(_dataSet.Screenshots, data.Image);
    }

    private bool IsDelayElapsed => _lastScreenshotTime + _parameters.ScreenshotingDelay <= DateTime.UtcNow;

    private bool ShouldMakeScreenshot(DetectionItem item)
    {
        return item.Probability >= _parameters.MinimumProbability && item.Probability <= _parameters.MaximumProbability;
    }
}