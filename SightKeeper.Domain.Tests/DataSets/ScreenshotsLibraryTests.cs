using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.DataSets.Detector;
using SightKeeper.Domain.Model.DataSets.Screenshots;

namespace SightKeeper.Domain.Tests.DataSets;

public sealed class ScreenshotsLibraryTests
{
	[Fact]
	public void ShouldNotDeleteScreenshotWithAsset()
	{
		ScreenshotsLibrary screenshotsLibrary = new();
		var screenshot = screenshotsLibrary.CreateScreenshot(DateTime.Now, new Vector2<ushort>(320, 320));
		DetectorDataSet dataSet = new();
		dataSet.AssetsLibrary.MakeAsset(screenshot);
		Assert.ThrowsAny<Exception>(() => screenshotsLibrary.RemoveScreenshotsRange(0, 1));
	}
}