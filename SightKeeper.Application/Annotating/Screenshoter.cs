using System.Diagnostics.CodeAnalysis;
using CommunityToolkit.Diagnostics;
using SightKeeper.Domain.Model;

namespace SightKeeper.Application.Annotating;

public sealed class Screenshoter
{
	public ScreenshotsLibrary? Library { get; set; }

	public Screenshoter(ScreenCapture screenCapture, SelfActivityService selfActivityService)
	{
		_screenCapture = screenCapture;
		_selfActivityService = selfActivityService;
	}
	
	public void MakeScreenshot()
	{
		if (!GetCanMakeScreenshot(out var message))
			ThrowHelper.ThrowInvalidOperationException($"Can't make screenshot: {message}");
		Guard.IsNotNull(_screenCapture.Resolution);
		var image = _screenCapture.Capture();
		Library.CreateScreenshot(image);
	}

	[MemberNotNullWhen(true, nameof(Library))]
	public bool GetCanMakeScreenshot([NotNullWhen(false)] out string? message)
	{
		if (Library == null)
			message = "Screenshots library is not set";
		else if (_selfActivityService.IsOwnWindowActive)
			message = "Own window is active";
		else message = null;
		return message == null;
	}
	
	private readonly ScreenCapture _screenCapture;
	private readonly SelfActivityService _selfActivityService;
}