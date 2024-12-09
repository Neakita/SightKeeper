using SightKeeper.Domain.DataSets.Detector;
using SightKeeper.Domain.Screenshots;

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

	[Fact]
	public void ShouldDeleteScreenshotAfterAssetDeletion()
	{
		ScreenshotsLibrary screenshotsLibrary = new();
		var screenshot = screenshotsLibrary.CreateScreenshot(DateTime.Now, new Vector2<ushort>(320, 320));
		DetectorDataSet dataSet = new();
		dataSet.AssetsLibrary.MakeAsset(screenshot);
		dataSet.AssetsLibrary.DeleteAsset(screenshot);
		screenshotsLibrary.RemoveScreenshotsRange(0, 1);
	}

	[Fact]
	public void ShouldDeleteScreenshotAfterAssetsLibraryClear()
	{
		ScreenshotsLibrary screenshotsLibrary = new();
		var screenshot = screenshotsLibrary.CreateScreenshot(DateTime.Now, new Vector2<ushort>(320, 320));
		DetectorDataSet dataSet = new();
		dataSet.AssetsLibrary.MakeAsset(screenshot);
		dataSet.AssetsLibrary.ClearAssets();
		screenshotsLibrary.RemoveScreenshotsRange(0, 1);
	}
}