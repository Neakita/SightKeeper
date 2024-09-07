using FluentAssertions;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.DataSets.Detector;

namespace SightKeeper.Domain.Tests.DataSets.Detector;

public sealed class DetectorAssetLibraryTests
{
	[Fact]
	public void ShouldCreateAsset()
	{
		DetectorDataSet dataSet = new();
		var screenshot = dataSet.ScreenshotsLibrary.CreateScreenshot(DateTime.Now, new Vector2<ushort>(320, 320), out _);
		var asset = dataSet.AssetsLibrary.MakeAsset(screenshot);
		screenshot.Asset.Should().Be(asset);
		dataSet.AssetsLibrary.Assets.Should().Contain(asset);
	}

	[Fact]
	public void ShouldNotCreateDuplicateAsset()
	{
		DetectorDataSet dataSet = new();
		var screenshot = dataSet.ScreenshotsLibrary.CreateScreenshot(DateTime.Now, new Vector2<ushort>(320, 320), out _);
		var asset = dataSet.AssetsLibrary.MakeAsset(screenshot);
		Assert.ThrowsAny<Exception>(() => dataSet.AssetsLibrary.MakeAsset(screenshot));
		screenshot.Asset.Should().Be(asset);
		dataSet.AssetsLibrary.Assets.Should().Contain(asset);
		dataSet.AssetsLibrary.Assets.Should().HaveCount(1);
	}

	[Fact]
	public void ShouldDeleteAsset()
	{
		DetectorDataSet dataSet = new();
		var screenshot = dataSet.ScreenshotsLibrary.CreateScreenshot(DateTime.Now, new Vector2<ushort>(320, 320), out _);
		var asset = dataSet.AssetsLibrary.MakeAsset(screenshot);
		dataSet.AssetsLibrary.DeleteAsset(asset);
		dataSet.AssetsLibrary.Assets.Should().BeEmpty();
		screenshot.Asset.Should().BeNull();
	}

	[Fact]
	public void ShouldNotDeleteAssetFromOtherDataSet()
	{
		DetectorDataSet dataSet1 = new();
		DetectorDataSet dataSet2 = new();
		var screenshot = dataSet1.ScreenshotsLibrary.CreateScreenshot(DateTime.Now, new Vector2<ushort>(320, 320), out _);
		var asset = dataSet1.AssetsLibrary.MakeAsset(screenshot);
		Assert.ThrowsAny<Exception>(() => dataSet2.AssetsLibrary.DeleteAsset(asset));
		asset.Screenshot.Asset.Should().Be(asset);
	}
}