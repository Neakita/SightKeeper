using CommunityToolkit.Diagnostics;
using CommunityToolkit.HighPerformance;
using HotKeys.ActionRunners;
using HotKeys.Bindings;
using SightKeeper.Application.Screenshotting.Saving;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
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
			var imageData = _screenCapture.Capture(Resolution, Offset);
			_screenshotsSaver.CreateScreenshot(Library, imageData, DateTimeOffset.Now);
			contextEliminated = context.WaitForElimination(Timeout);
		}
	}

	private static Image<TPixel> LoadImage(ReadOnlySpan2D<TPixel> imageData)
	{
		if (imageData.TryGetSpan(out var span))
			return Image.LoadPixelData(span, imageData.Width, imageData.Height);
		Image<TPixel> image = new(imageData.Width, imageData.Height);
		for (int i = 0; i < imageData.Height; i++)
		{
			var source = imageData.GetRowSpan(i);
			var target = image.DangerousGetPixelRowMemory(i);
			source.CopyTo(target.Span);
		}
		return image;
	}
}