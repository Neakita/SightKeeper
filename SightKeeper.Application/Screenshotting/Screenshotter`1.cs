using System.Diagnostics;
using CommunityToolkit.Diagnostics;
using HotKeys.Bindings;
using HotKeys.Handlers.Contextual;
using SightKeeper.Application.Screenshotting.Saving;
using SightKeeper.Domain;

namespace SightKeeper.Application.Screenshotting;

public sealed class Screenshotter<TPixel> : Screenshotter
{
	public override Vector2<ushort> ImageSize
	{
		get => base.ImageSize;
		set
		{
			base.ImageSize = value;
			_screenshotsSaver.MaximumImageSize = value;
		}
	}

	public Screenshotter(
		ScreenCapture<TPixel> screenCapture,
		ScreenBoundsProvider screenBoundsProvider,
		BindingsManager bindingsManager,
		ScreenshotsSaver<TPixel> screenshotsSaver)
		: base(screenBoundsProvider, bindingsManager)
	{
		_screenCapture = screenCapture;
		_screenshotsSaver = screenshotsSaver;
		_screenshotsSaver.MaximumImageSize = ImageSize;
	}

	private readonly ScreenCapture<TPixel> _screenCapture;
	private readonly ScreenshotsSaver<TPixel> _screenshotsSaver;
	private ScreenshotsSaverSession<TPixel>? _session;

	protected override void Enable()
	{
		base.Enable();
		Guard.IsNotNull(Library);
		Guard.IsNull(_session);
		_session = _screenshotsSaver.AcquireSession(Library);
	}

	protected override void Disable()
	{
		base.Disable();
		Guard.IsNotNull(_session);
		_screenshotsSaver.ReleaseSession(_session);
		_session = null;
	}

	protected override void MakeScreenshots(ActionContext context)
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
			session.CreateScreenshot(imageData);
		} while (IsEnabled && context.IsAliveAfter(Timeout - processingStopwatch.Elapsed));
		processingStopwatch.Stop();
	}
}