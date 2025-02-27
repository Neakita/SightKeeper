using System.Diagnostics;
using CommunityToolkit.Diagnostics;
using HotKeys.Bindings;
using HotKeys.Handlers.Contextual;
using SightKeeper.Application.ScreenCapturing.Saving;
using SightKeeper.Domain;

namespace SightKeeper.Application.ScreenCapturing;

public sealed class HotKeyScreenCapture<TPixel> : HotKeyScreenCapture
{
	public override Vector2<ushort> ImageSize
	{
		get => base.ImageSize;
		set
		{
			base.ImageSize = value;
			_imageSaver.MaximumImageSize = value;
		}
	}

	public HotKeyScreenCapture(
		ScreenCapture<TPixel> screenCapture,
		ScreenBoundsProvider screenBoundsProvider,
		BindingsManager bindingsManager,
		ImageSaver<TPixel> imageSaver)
		: base(screenBoundsProvider, bindingsManager)
	{
		_screenCapture = screenCapture;
		_imageSaver = imageSaver;
		_imageSaver.MaximumImageSize = ImageSize;
	}

	private readonly ScreenCapture<TPixel> _screenCapture;
	private readonly ImageSaver<TPixel> _imageSaver;
	private ImageSaverSession<TPixel>? _session;

	protected override void Enable()
	{
		base.Enable();
		Guard.IsNotNull(Library);
		Guard.IsNull(_session);
		_session = _imageSaver.AcquireSession(Library);
	}

	protected override void Disable()
	{
		base.Disable();
		Guard.IsNotNull(_session);
		_imageSaver.ReleaseSession(_session);
		_session = null;
	}

	protected override void MakeImages(ActionContext context)
	{
		var session = _session;
		Guard.IsNotNull(session);
		var processingStopwatch = Stopwatch.StartNew();
		do
		{
			if (session is LimitedSession limitedSession && context.IsEliminatedAfterCompletion(limitedSession.Limit))
				return;
			processingStopwatch.Restart();
			var imageData = _screenCapture.Capture(ImageSize, Offset);
			session.CreateImage(imageData);
		} while (IsEnabled && context.IsAliveAfter(Timeout - processingStopwatch.Elapsed));
		processingStopwatch.Stop();
	}
}