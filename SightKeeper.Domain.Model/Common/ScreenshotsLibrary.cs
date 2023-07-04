using System.Diagnostics.CodeAnalysis;
using CommunityToolkit.Diagnostics;

namespace SightKeeper.Domain.Model.Common;

public class ScreenshotsLibrary
{
    public IReadOnlyCollection<Screenshot> Screenshots => _screenshots;

    internal ScreenshotsLibrary()
    {
        _screenshots = new List<Screenshot>();
    }

    public Screenshot CreateScreenshot(Image image)
    {
        Screenshot screenshot = new(this, image);
        _screenshots.Add(screenshot);
        return screenshot;
    }

    public virtual bool CanCreateScreenshot(Image image, [NotNullWhen(false)] out string? message)
    {
        message = null;
        if (Screenshots.Any(screenshot => screenshot.Image == image))
            message = "Screenshot already added";
        return message == null;
    }

    public void AddScreenshot(Screenshot screenshot)
    {
        if (!CanAddScreenshot(screenshot, out var message))
            ThrowHelper.ThrowInvalidOperationException(message);
        _screenshots.Add(screenshot);
    }

    public virtual bool CanAddScreenshot(Screenshot screenshot, [NotNullWhen(false)] out string? message)
    {
        message = null;
        if (Screenshots.Contains(screenshot))
            message = "Screenshot already added";
        return message == null;
    }
	
    public void DeleteScreenshot(Screenshot screenshot)
    {
        if (!_screenshots.Remove(screenshot))
            ThrowHelper.ThrowInvalidOperationException("Screenshot not found");
    }
	
    private readonly List<Screenshot> _screenshots;
}