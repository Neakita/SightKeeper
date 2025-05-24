using CommunityToolkit.Diagnostics;
using SightKeeper.Application.ScreenCapturing.Saving;

namespace SightKeeper.Application.ScreenCapturing;

public sealed class HotKeyScreenCapturer<TPixel> : HotKeyScreenCapturer
{
	public required ScreenCapturer<TPixel> ScreenCapturer { get; init; }
	public required ImageSaver<TPixel> ImageSaver { get; init; }
	public required SelfActivityProvider SelfActivityProvider { get; init; }

	protected override void MakeImage()
	{
		if (SelfActivityProvider.IsOwnWindowActive)
			return;
		if (ImageSaver is LimitedSaver { IsLimitReached: true })
			return;
		var set = Set;
		Guard.IsNotNull(set);
		var imageData = ScreenCapturer.Capture(ImageSize, Offset);
		ImageSaver.SaveImage(set, imageData);
	}

	protected override void ClearExceedImages()
	{
		if (ImageSaver is LimitedSaver limitedSaver)
			limitedSaver.Processing.Wait();
		base.ClearExceedImages();
	}
}