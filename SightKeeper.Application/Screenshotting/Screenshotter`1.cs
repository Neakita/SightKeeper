using System.Diagnostics;
using CommunityToolkit.Diagnostics;
using HotKeys.ActionRunners;
using HotKeys.Bindings;
using SightKeeper.Application.Screenshotting.Saving;
using SixLabors.ImageSharp.PixelFormats;

namespace SightKeeper.Application.Screenshotting;

public sealed class Screenshotter<TPixel> : Screenshotter
	where TPixel : unmanaged, IPixel<TPixel>
{
	public Screenshotter(
		ScreenCapture<TPixel> screenCapture,
		ScreenshotsDataAccess screenshotsDataAccess,
		ScreenBoundsProvider screenBoundsProvider,
		BindingsManager bindingsManager,
		ScreenshotsSaver<TPixel> screenshotsSaver)
		: base(screenshotsDataAccess, screenBoundsProvider, bindingsManager)
	{
		_screenCapture = screenCapture;
		_screenshotsSaver = screenshotsSaver;
	}

	private readonly ScreenCapture<TPixel> _screenCapture;
	private readonly ScreenshotsSaver<TPixel> _screenshotsSaver;

	protected override void MakeScreenshots(ActionContext context)
	{
		var contextEliminated = false;
		while (!contextEliminated && IsEnabled)
		{
			Guard.IsNotNull(Library);
			var stopwatch = Stopwatch.StartNew();
			var imageData = _screenCapture.Capture(Resolution, Offset);
			_screenshotsSaver.CreateScreenshot(Library, imageData, DateTimeOffset.Now);
			if (Timeout != null)
				contextEliminated = context.WaitForElimination(Timeout.Value - stopwatch.Elapsed);
			else
				contextEliminated = !context.Alive;
			stopwatch.Stop();
			Console.WriteLine($"Elapsed: {stopwatch.Elapsed.TotalMilliseconds:N1}ms");
		}
	}
}