using System.Diagnostics;
using CommunityToolkit.Diagnostics;
using HotKeys.Bindings;
using HotKeys.Handlers.Contextual;
using SightKeeper.Application.ScreenCapturing.Saving;

namespace SightKeeper.Application.ScreenCapturing;

public sealed class HotKeyScreenCapture<TPixel> : HotKeyScreenCapture
{
	public HotKeyScreenCapture(
		ScreenCapture<TPixel> screenCapture,
		ScreenBoundsProvider screenBoundsProvider,
		BindingsManager bindingsManager,
		ImageSaver<TPixel> imageSaver,
		SelfActivityProvider selfActivityProvider,
		ImagesCleaner imagesCleaner)
		: base(screenBoundsProvider, bindingsManager)
	{
		_screenCapture = screenCapture;
		_imageSaver = imageSaver;
		_selfActivityProvider = selfActivityProvider;
		_imagesCleaner = imagesCleaner;
	}

	private readonly ScreenCapture<TPixel> _screenCapture;
	private readonly ImageSaver<TPixel> _imageSaver;
	private readonly SelfActivityProvider _selfActivityProvider;
	private readonly ImagesCleaner _imagesCleaner;

	protected override void MakeImages(ActionContext context)
	{
		if (_selfActivityProvider.IsOwnWindowActive)
			return;
		Guard.IsNotNull(Set);
		var set = Set;
		var processingStopwatch = Stopwatch.StartNew();
		do
		{
			if (_imageSaver is LimitedSaver limitedSession && context.IsEliminatedAfterCompletion(limitedSession.Limit))
				break;
			processingStopwatch.Restart();
			var imageData = _screenCapture.Capture(ImageSize, Offset);
			_imageSaver.SaveImage(set, imageData);
		} while (IsEnabled && context.IsAliveAfter(Timeout - processingStopwatch.Elapsed));
		processingStopwatch.Stop();
		_imagesCleaner.RemoveExceedUnusedImages(set);
	}
}