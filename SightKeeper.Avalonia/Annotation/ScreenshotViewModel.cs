using Avalonia.Media.Imaging;
using SightKeeper.Application;
using SightKeeper.Domain.Model.DataSets.Screenshots;

namespace SightKeeper.Avalonia.Annotation;

internal sealed class ScreenshotViewModel : ViewModel
{
	public Bitmap Image => new(_screenshotsDataAccess.LoadImage(_screenshot));

	public ScreenshotViewModel(Screenshot screenshot, ScreenshotsDataAccess screenshotsDataAccess)
	{
		_screenshot = screenshot;
		_screenshotsDataAccess = screenshotsDataAccess;
	}

	private readonly Screenshot _screenshot;
	private readonly ScreenshotsDataAccess _screenshotsDataAccess;
}