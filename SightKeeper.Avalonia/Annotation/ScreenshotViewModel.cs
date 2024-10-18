using Avalonia.Media.Imaging;
using SightKeeper.Application.Screenshotting;
using SightKeeper.Domain.Model.DataSets.Screenshots;

namespace SightKeeper.Avalonia.Annotation;

internal sealed class ScreenshotViewModel : ViewModel
{
	public Bitmap Image => new(_screenshotsDataAccess.LoadImage(_screenshot));
	public Bitmap PreviewImage => Bitmap.DecodeToWidth(
		_screenshotsDataAccess.LoadImage(_screenshot),
		100,
		BitmapInterpolationMode.LowQuality);

	public ScreenshotViewModel(Screenshot screenshot, ScreenshotsDataAccess screenshotsDataAccess)
	{
		_screenshot = screenshot;
		_screenshotsDataAccess = screenshotsDataAccess;
	}

	private readonly Screenshot _screenshot;
	private readonly ScreenshotsDataAccess _screenshotsDataAccess;
}