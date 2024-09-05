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
		dataSet.Screenshots.MaxQuantity = 2;
		var screenshot1 = dataSet.Screenshots.CreateScreenshot(DateTime.Now, new Vector2<ushort>(320, 320), out var removedScreenshots);
		removedScreenshots.Should().BeEmpty();
		var screenshot2 = dataSet.Screenshots.CreateScreenshot(DateTime.Now, new Vector2<ushort>(320, 320), out removedScreenshots);
		removedScreenshots.Should().BeEmpty();
		var screenshot3 = dataSet.Screenshots.CreateScreenshot(DateTime.Now, new Vector2<ushort>(320, 320), out removedScreenshots);
		removedScreenshots.Should().Contain(screenshot1);
		dataSet.Screenshots.Should().NotContain(screenshot1);
		dataSet.Screenshots.Should().ContainInOrder(screenshot2, screenshot3);
	}
	
	[Fact]
	public void ShouldNotDeleteScreenshotWithAsset()
	{
		DetectorDataSet dataSet = new();
		dataSet.Screenshots.MaxQuantity = 2;
		var screenshot1 = dataSet.Screenshots.CreateScreenshot(DateTime.Now, new Vector2<ushort>(320, 320), out var removedScreenshots);
		removedScreenshots.Should().BeEmpty();
		var screenshot2 = dataSet.Screenshots.CreateScreenshot(DateTime.Now, new Vector2<ushort>(320, 320), out removedScreenshots);
		removedScreenshots.Should().BeEmpty();
		dataSet.Assets.MakeAsset(screenshot2);
		var screenshot3 = dataSet.Screenshots.CreateScreenshot(DateTime.Now, new Vector2<ushort>(320, 320), out removedScreenshots);
		removedScreenshots.Should().BeEmpty();
		dataSet.Screenshots.Should().ContainInOrder(screenshot1, screenshot2, screenshot3);
	}
}