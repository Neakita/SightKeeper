using FluentAssertions;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.DataSets.Detector;
using SightKeeper.Domain.Model.DataSets.Screenshots;

namespace SightKeeper.Domain.Tests.DataSets.Detector;

public sealed class DetectorAssetLibraryTests
{
	[Fact]
	public void ShouldCreateAsset()
	{
		ScreenshotsLibrary screenshotsLibrary = new();
		var screenshot = screenshotsLibrary.CreateScreenshot(DateTimeOffset.Now, new Vector2<ushort>(320, 320));
		DetectorDataSet dataSet = new();
		var asset = dataSet.AssetsLibrary.MakeAsset(screenshot);
		dataSet.AssetsLibrary.Assets.Should().ContainValue(asset);
	}

	[Fact]
	public void ShouldNotCreateDuplicateAsset()
	{
		ScreenshotsLibrary screenshotsLibrary = new();
		var screenshot = screenshotsLibrary.CreateScreenshot(DateTimeOffset.Now, new Vector2<ushort>(320, 320));
		DetectorDataSet dataSet = new();
		var asset = dataSet.AssetsLibrary.MakeAsset(screenshot);
		Assert.ThrowsAny<Exception>(() => dataSet.AssetsLibrary.MakeAsset(screenshot));
		dataSet.AssetsLibrary.Assets.Should().ContainValue(asset);
		dataSet.AssetsLibrary.Assets.Should().HaveCount(1);
	}

	[Fact]
	public void ShouldDeleteAsset()
	{
		ScreenshotsLibrary screenshotsLibrary = new();
		var screenshot = screenshotsLibrary.CreateScreenshot(DateTimeOffset.Now, new Vector2<ushort>(320, 320));
		DetectorDataSet dataSet = new();
		dataSet.AssetsLibrary.MakeAsset(screenshot);
		dataSet.AssetsLibrary.DeleteAsset(screenshot);
		dataSet.AssetsLibrary.Assets.Should().BeEmpty();
	}

	[Fact]
	public void ShouldNotDeleteAssetFromOtherDataSet()
	{
		ScreenshotsLibrary screenshotsLibrary = new();
		var screenshot = screenshotsLibrary.CreateScreenshot(DateTimeOffset.Now, new Vector2<ushort>(320, 320));
		DetectorDataSet dataSet1 = new();
		DetectorDataSet dataSet2 = new();
		dataSet1.AssetsLibrary.MakeAsset(screenshot);
		Assert.ThrowsAny<Exception>(() => dataSet2.AssetsLibrary.DeleteAsset(screenshot));
	}
}