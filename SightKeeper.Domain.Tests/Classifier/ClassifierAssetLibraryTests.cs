using FluentAssertions;
using SightKeeper.Domain.Model.DataSets.Classifier;

namespace SightKeeper.Domain.Tests.Classifier;

public sealed class ClassifierAssetLibraryTests
{
	[Fact]
	public void ShouldCreateAsset()
	{
		ClassifierDataSet dataSet = new("", 320);
		var tag = dataSet.Tags.CreateTag("");
		SimpleClassifierScreenshotsDataAccess screenshotsDataAccess = new();
		var screenshot = screenshotsDataAccess.CreateScreenshot(dataSet, []);
		var asset = dataSet.Assets.MakeAsset(screenshot, tag);
		screenshot.Asset.Should().Be(asset);
		dataSet.Assets.Should().Contain(asset);
	}

	[Fact]
	public void ShouldNotCreateDuplicateAsset()
	{
		ClassifierDataSet dataSet = new("", 320);
		var tag = dataSet.Tags.CreateTag("");
		SimpleClassifierScreenshotsDataAccess screenshotsDataAccess = new();
		var screenshot = screenshotsDataAccess.CreateScreenshot(dataSet, []);
		var asset = dataSet.Assets.MakeAsset(screenshot, tag);
		Assert.ThrowsAny<Exception>(() => dataSet.Assets.MakeAsset(screenshot, tag));
		screenshot.Asset.Should().Be(asset);
		dataSet.Assets.Should().Contain(asset);
		dataSet.Assets.Should().HaveCount(1);
	}

	[Fact]
	public void ShouldRemoveAssetFromScreenshot()
	{
		ClassifierDataSet dataSet = new("", 320);
		var tag = dataSet.Tags.CreateTag("");
		SimpleClassifierScreenshotsDataAccess screenshotsDataAccess = new();
		var screenshot = screenshotsDataAccess.CreateScreenshot(dataSet, []);
		var asset = dataSet.Assets.MakeAsset(screenshot, tag);
		dataSet.Assets.DeleteAsset(asset);
		screenshot.Asset.Should().BeNull();
	}

	[Fact]
	public void ShouldNotDeleteAssetFromOtherDataSet()
	{
		ClassifierDataSet dataSet1 = new("", 320);
		var tag = dataSet1.Tags.CreateTag("");
		ClassifierDataSet dataSet2 = new("", 320);
		SimpleClassifierScreenshotsDataAccess screenshotsDataAccess = new();
		var screenshot = screenshotsDataAccess.CreateScreenshot(dataSet1, []);
		var asset = dataSet1.Assets.MakeAsset(screenshot, tag);
		Assert.ThrowsAny<Exception>(() => dataSet2.Assets.DeleteAsset(asset));
		asset.Screenshot.Asset.Should().Be(asset);
	}
	
	[Fact]
	public void ShouldNotCreateAssetWithForeignTag()
	{
		ClassifierDataSet dataSet = new("", 320);
		var foreignTag = new ClassifierDataSet("", 320).Tags.CreateTag("");
		SimpleClassifierScreenshotsDataAccess screenshotsDataAccess = new();
		var screenshot = screenshotsDataAccess.CreateScreenshot(dataSet, []);
		Assert.ThrowsAny<Exception>(() => dataSet.Assets.MakeAsset(screenshot, foreignTag));
		dataSet.Assets.Should().BeEmpty();
	}
}