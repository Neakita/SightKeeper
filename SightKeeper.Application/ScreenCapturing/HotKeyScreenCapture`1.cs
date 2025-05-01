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
		SelfActivityProvider selfActivityProvider)
		: base(screenBoundsProvider, bindingsManager)
	{
		_screenCapture = screenCapture;
		_imageSaver = imageSaver;
		_selfActivityProvider = selfActivityProvider;
	}

	private readonly ScreenCapture<TPixel> _screenCapture;
	private readonly ImageSaver<TPixel> _imageSaver;
	private readonly SelfActivityProvider _selfActivityProvider;

	protected override void MakeImages(ActionContext context)
	{
		if (_selfActivityProvider.IsOwnWindowActive)
			return;
		var processingStopwatch = Stopwatch.StartNew();
		do
		{
			if (_imageSaver is LimitedSaver limitedSession && context.IsEliminatedAfterCompletion(limitedSession.Limit))
				return;
			processingStopwatch.Restart();
			var imageData = _screenCapture.Capture(ImageSize, Offset);
			Guard.IsNotNull(Set);
			_imageSaver.SaveImage(Set, imageData);
		} while (IsEnabled && context.IsAliveAfter(Timeout - processingStopwatch.Elapsed));
		processingStopwatch.Stop();
	}
}