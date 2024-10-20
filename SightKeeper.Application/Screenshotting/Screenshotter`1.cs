using System.Diagnostics;
using CommunityToolkit.Diagnostics;
using HotKeys.ActionRunners;
using HotKeys.Bindings;
using SightKeeper.Application.Screenshotting.Saving;
using SightKeeper.Domain.Model;
using SixLabors.ImageSharp.PixelFormats;

namespace SightKeeper.Application.Screenshotting;

public sealed class Screenshotter<TPixel> : Screenshotter
	where TPixel : unmanaged, IPixel<TPixel>
{
	public override Vector2<ushort> Resolution
	{
		get => base.Resolution;
		set
		{
			base.Resolution = value;
			_screenshotsSaver.ImageSize = value;
		}
	}

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
		_screenshotsSaver.ImageSize = Resolution;
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
		var contextEliminated = false;
		Guard.IsNotNull(_session);
		var session = _session;
		while (!contextEliminated && IsEnabled)
		{
			var stopwatch = Stopwatch.StartNew();
			var imageData = _screenCapture.Capture(Resolution, Offset);
			session.CreateScreenshot(imageData, DateTimeOffset.Now);
			if (Timeout != null)
				contextEliminated = context.WaitForElimination(Timeout.Value - stopwatch.Elapsed);
			else
				contextEliminated = !context.Alive;
			stopwatch.Stop();
			Console.WriteLine($"Elapsed: {stopwatch.Elapsed.TotalMilliseconds:N1}ms");
		}
	}
}