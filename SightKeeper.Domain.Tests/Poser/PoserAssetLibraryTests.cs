using FluentAssertions;
using SightKeeper.Domain.Model.DataSets.Poser;

namespace SightKeeper.Domain.Tests.Poser;

public sealed class PoserAssetLibraryTests
{
	[Fact]
	public void ShouldCreateAsset()
	{
		PoserDataSet dataSet = new("", 320);
		SimplePoserScreenshotsDataAccess screenshotsDataAccess = new();
		var screenshot = screenshotsDataAccess.CreateScreenshot(dataSet, []);
		var asset = dataSet.Assets.MakeAsset(screenshot);
		screenshot.Asset.Should().Be(asset);
		dataSet.Assets.Should().Contain(asset);
	}

	[Fact]
	public void ShouldNotCreateDuplicateAsset()
	{
		PoserDataSet dataSet = new("", 320);
		SimplePoserScreenshotsDataAccess screenshotsDataAccess = new();
		var screenshot = screenshotsDataAccess.CreateScreenshot(dataSet, []);
		var asset = dataSet.Assets.MakeAsset(screenshot);
		Assert.ThrowsAny<Exception>(() => dataSet.Assets.MakeAsset(screenshot));
		screenshot.Asset.Should().Be(asset);
		dataSet.Assets.Should().Contain(asset);
		dataSet.Assets.Should().HaveCount(1);
	}

	[Fact]
	public void ShouldDeleteAsset()
	{
		PoserDataSet dataSet = new("", 320);
		SimplePoserScreenshotsDataAccess screenshotsDataAccess = new();
		var screenshot = screenshotsDataAccess.CreateScreenshot(dataSet, []);
		var asset = dataSet.Assets.MakeAsset(screenshot);
		dataSet.Assets.DeleteAsset(asset);
		dataSet.Assets.Should().BeEmpty();
		screenshot.Asset.Should().BeNull();
	}

	[Fact]
	public void ShouldNotDeleteAssetFromOtherDataSet()
	{
		PoserDataSet dataSet1 = new("", 320);
		PoserDataSet dataSet2 = new("", 320);
		SimplePoserScreenshotsDataAccess screenshotsDataAccess = new();
		var screenshot = screenshotsDataAccess.CreateScreenshot(dataSet1, []);
		var asset = dataSet1.Assets.MakeAsset(screenshot);
		Assert.ThrowsAny<Exception>(() => dataSet2.Assets.DeleteAsset(asset));
		asset.Screenshot.Asset.Should().Be(asset);
	}
}