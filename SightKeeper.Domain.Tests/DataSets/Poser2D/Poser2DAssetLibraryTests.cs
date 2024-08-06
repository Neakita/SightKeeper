using FluentAssertions;
using SightKeeper.Domain.Model.DataSets.Poser2D;

namespace SightKeeper.Domain.Tests.DataSets.Poser2D;

public sealed class Poser2DAssetLibraryTests
{
	[Fact]
	public void ShouldCreateAsset()
	{
		Poser2DDataSet dataSet = new("", 320);
		SimpleScreenshotsDataAccess screenshotsDataAccess = new();
		var screenshot = screenshotsDataAccess.CreateScreenshot(dataSet.Screenshots, []);
		var asset = dataSet.Assets.MakeAsset(screenshot);
		screenshot.Asset.Should().Be(asset);
		dataSet.Assets.Should().Contain(asset);
	}

	[Fact]
	public void ShouldNotCreateDuplicateAsset()
	{
		Poser2DDataSet dataSet = new("", 320);
		SimpleScreenshotsDataAccess screenshotsDataAccess = new();
		var screenshot = screenshotsDataAccess.CreateScreenshot(dataSet.Screenshots, []);
		var asset = dataSet.Assets.MakeAsset(screenshot);
		Assert.ThrowsAny<Exception>(() => dataSet.Assets.MakeAsset(screenshot));
		screenshot.Asset.Should().Be(asset);
		dataSet.Assets.Should().Contain(asset);
		dataSet.Assets.Should().HaveCount(1);
	}

	[Fact]
	public void ShouldDeleteAsset()
	{
		Poser2DDataSet dataSet = new("", 320);
		SimpleScreenshotsDataAccess screenshotsDataAccess = new();
		var screenshot = screenshotsDataAccess.CreateScreenshot(dataSet.Screenshots, []);
		var asset = dataSet.Assets.MakeAsset(screenshot);
		dataSet.Assets.DeleteAsset(asset);
		dataSet.Assets.Should().BeEmpty();
		screenshot.Asset.Should().BeNull();
	}

	[Fact]
	public void ShouldNotDeleteAssetFromOtherDataSet()
	{
		Poser2DDataSet dataSet1 = new("", 320);
		Poser2DDataSet dataSet2 = new("", 320);
		SimpleScreenshotsDataAccess screenshotsDataAccess = new();
		var screenshot = screenshotsDataAccess.CreateScreenshot(dataSet1.Screenshots, []);
		var asset = dataSet1.Assets.MakeAsset(screenshot);
		Assert.ThrowsAny<Exception>(() => dataSet2.Assets.DeleteAsset(asset));
		asset.Screenshot.Asset.Should().Be(asset);
	}
}