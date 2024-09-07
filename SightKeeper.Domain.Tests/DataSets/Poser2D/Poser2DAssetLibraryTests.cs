using FluentAssertions;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.DataSets.Poser2D;

namespace SightKeeper.Domain.Tests.DataSets.Poser2D;

public sealed class Poser2DAssetLibraryTests
{
	[Fact]
	public void ShouldCreateAsset()
	{
		Poser2DDataSet dataSet = new();
		var screenshot = dataSet.ScreenshotsLibrary.CreateScreenshot(DateTime.Now, new Vector2<ushort>(320, 320), out _);
		var asset = dataSet.AssetsLibrary.MakeAsset(screenshot);
		screenshot.Asset.Should().Be(asset);
		dataSet.AssetsLibrary.Assets.Should().Contain(asset);
	}

	[Fact]
	public void ShouldNotCreateDuplicateAsset()
	{
		Poser2DDataSet dataSet = new();
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
		Poser2DDataSet dataSet = new();
		var screenshot = dataSet.ScreenshotsLibrary.CreateScreenshot(DateTime.Now, new Vector2<ushort>(320, 320), out _);
		var asset = dataSet.AssetsLibrary.MakeAsset(screenshot);
		dataSet.AssetsLibrary.DeleteAsset(asset);
		dataSet.AssetsLibrary.Assets.Should().BeEmpty();
		screenshot.Asset.Should().BeNull();
	}

	[Fact]
	public void ShouldNotDeleteAssetFromOtherDataSet()
	{
		Poser2DDataSet dataSet1 = new();
		Poser2DDataSet dataSet2 = new();
		var screenshot = dataSet1.ScreenshotsLibrary.CreateScreenshot(DateTime.Now, new Vector2<ushort>(320, 320), out _);
		var asset = dataSet1.AssetsLibrary.MakeAsset(screenshot);
		Assert.ThrowsAny<Exception>(() => dataSet2.AssetsLibrary.DeleteAsset(asset));
		asset.Screenshot.Asset.Should().Be(asset);
	}
}