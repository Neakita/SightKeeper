using CommunityToolkit.Diagnostics;
using SightKeeper.Domain.Model;

namespace SightKeeper.Application.Annotating;

public sealed class Screenshoter
{
	public ScreenshotsLibrary? Library { get; set; }

	public Screenshoter(ScreenCapture screenCapture)
	{
		_screenCapture = screenCapture;
	}
	
	public void MakeScreenshot()
	{
		Guard.IsNotNull(Library);
		var image = _screenCapture.Capture();
		Library.CreateScreenshot(image);
	}
	
	private readonly ScreenCapture _screenCapture;
}