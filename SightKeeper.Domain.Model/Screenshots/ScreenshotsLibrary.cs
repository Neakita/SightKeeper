using CommunityToolkit.Diagnostics;
using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Domain.Model;

public sealed class ScreenshotsLibrary
{
    public ushort? MaxQuantity { get; set; }
    public IReadOnlyCollection<Screenshot> Screenshots => _screenshots;
    public bool HasAnyScreenshots { get; private set; }

    internal ScreenshotsLibrary()
    {
        _screenshots = new List<Screenshot>();
    }

    public Screenshot CreateScreenshot(byte[] content, Resolution resolution)
    {
        Screenshot screenshot = new(content, resolution);
        _screenshots.Add(screenshot);
        ClearExceed();
        HasAnyScreenshots = Screenshots.Any();
        return screenshot;
    }
	
    public void DeleteScreenshot(Screenshot screenshot)
    {
        if (!_screenshots.Remove(screenshot))
            ThrowHelper.ThrowInvalidOperationException("Screenshot not found");
        HasAnyScreenshots = Screenshots.Any();
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