using CommunityToolkit.Diagnostics;

namespace SightKeeper.Domain.Model.DataSets;

public sealed class ScreenshotsLibrary
{
    public ushort? MaxQuantity { get; set; }
    public IReadOnlySet<Screenshot> Screenshots => _screenshots;

    public Screenshot CreateScreenshot(byte[] content)
    {
        Screenshot screenshot = new(content);
        _screenshots.Add(screenshot);
        ClearExceed();
        return screenshot;
    }

    public void DeleteScreenshot(Screenshot screenshot)
    {
        if (!_screenshots.Remove(screenshot))
            ThrowHelper.ThrowInvalidOperationException("Screenshot not found");
        screenshot.Asset?.ClearItems();
    }

    private readonly SortedSet<Screenshot> _screenshots = new(ScreenshotsDateComparer.Instance);

    private void ClearExceed()
    {
        if (MaxQuantity == null)
            return;
        var exceedAmount = _screenshots.Count - MaxQuantity.Value;
        if (exceedAmount <= 0)
	        return;
        var screenshotsToDelete = _screenshots.Take(exceedAmount);
        foreach (var screenshot in screenshotsToDelete)
	        _screenshots.Remove(screenshot);
    }
}