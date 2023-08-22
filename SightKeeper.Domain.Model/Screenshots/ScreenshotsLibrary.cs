using System.Reactive.Linq;
using System.Reactive.Subjects;
using CommunityToolkit.Diagnostics;
using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Domain.Model;

public sealed class ScreenshotsLibrary
{
    public IObservable<Screenshot> ScreenshotAdded => _screenshotAdded.AsObservable();
    private readonly Subject<Screenshot> _screenshotAdded = new();
    public IObservable<Screenshot> ScreenshotRemoved => _screenshotRemoved.AsObservable();
    private readonly Subject<Screenshot> _screenshotRemoved = new();

    public DataSet DataSet { get; private set; }
    public ushort? MaxQuantity { get; set; }
    public IReadOnlyCollection<Screenshot> Screenshots => _screenshots;
    public bool HasAnyScreenshots { get; private set; }

    internal ScreenshotsLibrary(DataSet dataSet)
    {
        DataSet = dataSet;
        _screenshots = new List<Screenshot>();
    }

    public Screenshot CreateScreenshot(byte[] content, Resolution resolution)
    {
        Screenshot screenshot = new(this, content, resolution);
        _screenshots.Add(screenshot);
        ClearExceed();
        HasAnyScreenshots = Screenshots.Any();
        _screenshotAdded.OnNext(screenshot);
        return screenshot;
    }
	
    public void DeleteScreenshot(Screenshot screenshot)
    {
        if (!_screenshots.Remove(screenshot))
            ThrowHelper.ThrowInvalidOperationException("Screenshot not found");
        HasAnyScreenshots = Screenshots.Any();
        _screenshotRemoved.OnNext(screenshot);
    }
	
    private readonly List<Screenshot> _screenshots;

    private void ClearExceed()
    {
        if (MaxQuantity == null)
            return;
        var screenshotsToDelete = Screenshots
            .Where(screenshot => screenshot.Asset == null)
            .OrderByDescending(screenshot => screenshot.CreationDate)
            .Skip(MaxQuantity.Value)
            .ToList();
        foreach (var screenshot in screenshotsToDelete)
            DeleteScreenshot(screenshot);
    }
}