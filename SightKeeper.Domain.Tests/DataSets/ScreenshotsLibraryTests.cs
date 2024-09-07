using FluentAssertions;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.DataSets.Detector;

namespace SightKeeper.Domain.Tests.DataSets;

public sealed class ScreenshotsLibraryTests
{
	[Fact]
	public void ShouldDeleteExceedScreenshot()
	{
		DetectorDataSet dataSet = new();
		dataSet.ScreenshotsLibrary.MaxQuantity = 2;
		var screenshot1 = dataSet.ScreenshotsLibrary.CreateScreenshot(DateTime.Now, new Vector2<ushort>(320, 320), out var removedScreenshots);
		removedScreenshots.Should().BeEmpty();
		var screenshot2 = dataSet.ScreenshotsLibrary.CreateScreenshot(DateTime.Now, new Vector2<ushort>(320, 320), out removedScreenshots);
		removedScreenshots.Should().BeEmpty();
		var screenshot3 = dataSet.ScreenshotsLibrary.CreateScreenshot(DateTime.Now, new Vector2<ushort>(320, 320), out removedScreenshots);
		removedScreenshots.Should().Contain(screenshot1);
		dataSet.ScreenshotsLibrary.Screenshots.Should().NotContain(screenshot1);
		dataSet.ScreenshotsLibrary.Screenshots.Should().ContainInOrder(screenshot2, screenshot3);
	}
	
	[Fact]
	public void ShouldNotDeleteScreenshotWithAsset()
	{
		DetectorDataSet dataSet = new();
		dataSet.ScreenshotsLibrary.MaxQuantity = 2;
		var screenshot1 = dataSet.ScreenshotsLibrary.CreateScreenshot(DateTime.Now, new Vector2<ushort>(320, 320), out var removedScreenshots);
		removedScreenshots.Should().BeEmpty();
		var screenshot2 = dataSet.ScreenshotsLibrary.CreateScreenshot(DateTime.Now, new Vector2<ushort>(320, 320), out removedScreenshots);
		removedScreenshots.Should().BeEmpty();
		dataSet.AssetsLibrary.MakeAsset(screenshot2);
		var screenshot3 = dataSet.ScreenshotsLibrary.CreateScreenshot(DateTime.Now, new Vector2<ushort>(320, 320), out removedScreenshots);
		removedScreenshots.Should().BeEmpty();
		dataSet.ScreenshotsLibrary.Screenshots.Should().ContainInOrder(screenshot1, screenshot2, screenshot3);
	}
}