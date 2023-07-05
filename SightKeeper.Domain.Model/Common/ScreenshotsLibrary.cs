using CommunityToolkit.Diagnostics;

namespace SightKeeper.Domain.Model.Common;

public class ScreenshotsLibrary
{
    public IReadOnlyCollection<Screenshot> Screenshots => _screenshots;

    public ScreenshotsLibrary()
    {
        _screenshots = new List<Screenshot>();
    }

    public Screenshot CreateScreenshot(Image image)
    {
        if (image.Screenshot != null)
            ThrowHelper.ThrowArgumentException("Image already added as screenshot");
        Screenshot screenshot = new(this, image);
        _screenshots.Add(screenshot);
        image.Screenshot = screenshot;
        return screenshot;
    }

    public void AddScreenshot(Screenshot screenshot)
    {
        if (screenshot.Library != null)
            ThrowHelper.ThrowArgumentException("Screenshot already added to library");
        _screenshots.Add(screenshot);
        screenshot.Library = this;
    }
	
    public void DeleteScreenshot(Screenshot screenshot)
    {
        if (!_screenshots.Remove(screenshot))
            ThrowHelper.ThrowInvalidOperationException("Screenshot not found");
        screenshot.Library = null;
    }
	
    private readonly List<Screenshot> _screenshots;
}