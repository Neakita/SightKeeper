using System.Diagnostics.CodeAnalysis;
using CommunityToolkit.Diagnostics;

namespace SightKeeper.Domain.Model.Common;

public class ScreenshotsLibrary
{
    public IReadOnlyCollection<Screenshot> Screenshots => _screenshots;

    public ScreenshotsLibrary()
    {
        _screenshots = new List<Screenshot>();
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