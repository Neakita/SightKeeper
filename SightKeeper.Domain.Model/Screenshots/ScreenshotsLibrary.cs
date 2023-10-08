using System.Reactive.Linq;
using System.Reactive.Subjects;
using CommunityToolkit.Diagnostics;
using FlakeId;

namespace SightKeeper.Domain.Model;

public sealed class ScreenshotsLibrary
{
    public Id Id { get; private set; }
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

    public Screenshot CreateScreenshot(byte[] content)
    {
        Screenshot screenshot = new(content, this);
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
        screenshot.Asset?.ClearItems();
        _screenshotRemoved.OnNext(screenshot);
    }

    public void DeleteScreenshot(int screenshotIndex)
    {
        var screenshot = _screenshots[screenshotIndex];
        _screenshots.RemoveAt(screenshotIndex);
        HasAnyScreenshots = Screenshots.Any();
        screenshot.Asset?.ClearItems();
        _screenshotRemoved.OnNext(screenshot);
    }
	
    private readonly List<Screenshot> _screenshots;

    private ScreenshotsLibrary()
    {
        DataSet = null!;
        _screenshots = null!;
    }

    private void ClearExceed()
    {
        if (MaxQuantity == null)
            return;
        var screenshotsToDelete = Screenshots
            .Select((item, index) => (item, index))
            .Where(screenshot => screenshot.item.Asset == null)
            .OrderByDescending(screenshot => screenshot.item.CreationDate)
            .Skip(MaxQuantity.Value)
            .OrderByDescending(screenshot => screenshot.index)
            .ToList();
        foreach (var screenshot in screenshotsToDelete)
            DeleteScreenshot(screenshot.index);
    }
}