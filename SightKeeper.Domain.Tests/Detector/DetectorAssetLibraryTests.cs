using FluentAssertions;
using SightKeeper.Domain.Model.DataSets.Detector;

namespace SightKeeper.Domain.Tests.Detector;

public sealed class DetectorAssetLibraryTests
{
	[Fact]
	public void ShouldCreateAsset()
	{
		DetectorDataSet dataSet = new("", 320);
		SimpleDetectorScreenshotsDataAccess detectorScreenshotsDataAccess = new();
		var screenshot = detectorScreenshotsDataAccess.CreateScreenshot(dataSet, []);
		var asset = dataSet.Assets.MakeAsset(screenshot);
		screenshot.Asset.Should().Be(asset);
		dataSet.Assets.Should().Contain(asset);
	}

	[Fact]
	public void ShouldNotCreateDuplicateAsset()
	{
		DetectorDataSet dataSet = new("", 320);
		SimpleDetectorScreenshotsDataAccess detectorScreenshotsDataAccess = new();
		var screenshot = detectorScreenshotsDataAccess.CreateScreenshot(dataSet, []);
		var asset = dataSet.Assets.MakeAsset(screenshot);
		Assert.ThrowsAny<Exception>(() => dataSet.Assets.MakeAsset(screenshot));
		screenshot.Asset.Should().Be(asset);
		dataSet.Assets.Should().Contain(asset);
		dataSet.Assets.Should().HaveCount(1);
	}

	[Fact]
	public void ShouldRemoveAssetFromScreenshot()
	{
		DetectorDataSet dataSet = new("", 320);
		SimpleDetectorScreenshotsDataAccess detectorScreenshotsDataAccess = new();
		var screenshot = detectorScreenshotsDataAccess.CreateScreenshot(dataSet, []);
		var asset = dataSet.Assets.MakeAsset(screenshot);
		dataSet.Assets.DeleteAsset(asset);
		screenshot.Asset.Should().BeNull();
	}

	[Fact]
	public void ShouldNotDeleteAssetFromOtherDataSet()
	{
		DetectorDataSet dataSet1 = new("", 320);
		DetectorDataSet dataSet2 = new("", 320);
		SimpleDetectorScreenshotsDataAccess detectorScreenshotsDataAccess = new();
		var screenshot = detectorScreenshotsDataAccess.CreateScreenshot(dataSet1, []);
		var asset = dataSet1.Assets.MakeAsset(screenshot);
		Assert.ThrowsAny<Exception>(() => dataSet2.Assets.DeleteAsset(asset));
		asset.Screenshot.Asset.Should().Be(asset);
	}
}