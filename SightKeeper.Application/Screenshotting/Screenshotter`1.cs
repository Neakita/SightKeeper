using System.Diagnostics;
using CommunityToolkit.Diagnostics;
using HotKeys.ActionRunners;
using HotKeys.Bindings;
using SightKeeper.Application.Screenshotting.Saving;
using SightKeeper.Domain.Model;

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
		var contextEliminated = false;
		var session = _session;
		Guard.IsNotNull(session);
		Task[] waitTasks = [Task.CompletedTask, context.Elimination];
		var offset = Offset;
		while (!contextEliminated && IsEnabled)
		{
			if (_session is LimitedSession limitedSession)
			{
				waitTasks[0] = limitedSession.Limit;
				if (Task.WaitAny(waitTasks) == 1)
					return;
			}
			var stopwatch = Stopwatch.StartNew();
			var imageData = _screenCapture.Capture(ImageSize, offset);
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