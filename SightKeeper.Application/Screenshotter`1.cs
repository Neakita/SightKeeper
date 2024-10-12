using CommunityToolkit.Diagnostics;
using CommunityToolkit.HighPerformance;
using HotKeys.ActionRunners;
using HotKeys.Bindings;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.PixelFormats;

namespace SightKeeper.Application;

public sealed class Screenshotter<TPixel> : Screenshotter
	where TPixel : unmanaged, IPixel<TPixel>
{
	public Screenshotter(
		ScreenCapture<TPixel> screenCapture,
		ScreenshotsDataAccess screenshotsDataAccess,
		ScreenBoundsProvider screenBoundsProvider,
		BindingsManager bindingsManager)
		: base(screenshotsDataAccess, screenBoundsProvider, bindingsManager)
	{
		_screenCapture = screenCapture;
	}

	private readonly ScreenCapture<TPixel> _screenCapture;

	protected override void MakeScreenshots(ActionContext context)
	{
		var contextEliminated = false;
		while (!contextEliminated && IsEnabled)
		{
			Guard.IsNotNull(Library);
			var imageData = _screenCapture.Capture(Resolution, Offset);
			using var image = LoadImage(imageData);
			ScreenshotsDataAccess.CreateScreenshot(Library, image, DateTimeOffset.Now, Resolution);
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