using SightKeeper.Domain.Model.DataSets.Detector;

namespace SightKeeper.Domain.Tests;

public sealed class ScreenshotsLibraryTests
{
	[Fact]
	public void ShouldDeleteExceedScreenshot()
	{
		DetectorDataSet dataSet = new("", 320);
		dataSet.Screenshots.MaxQuantity = 2;
		SimpleDetectorScreenshotsDataAccess screenshotsDataAccess = new();
		var screenshot1 = screenshotsDataAccess.CreateScreenshot(dataSet, []);
		var screenshot2 = screenshotsDataAccess.CreateScreenshot(dataSet, []);
		var screenshot3 = screenshotsDataAccess.CreateScreenshot(dataSet, []);
		dataSet.Screenshots.Should().NotContain(screenshot1);
		dataSet.Screenshots.Should().ContainInOrder(screenshot2, screenshot3);
	}
	
	[Fact]
	public void ShouldNotDeleteScreenshotWithAsset()
	{
		DetectorDataSet dataSet = new("", 320);
		dataSet.Screenshots.MaxQuantity = 2;
		SimpleDetectorScreenshotsDataAccess screenshotsDataAccess = new();
		var screenshot1 = screenshotsDataAccess.CreateScreenshot(dataSet, []);
		var screenshot2 = screenshotsDataAccess.CreateScreenshot(dataSet, []);
		dataSet.Assets.MakeAsset(screenshot2);
		var screenshot3 = screenshotsDataAccess.CreateScreenshot(dataSet, []);
		dataSet.Screenshots.Should().ContainInOrder(screenshot1, screenshot2, screenshot3);
	}
}