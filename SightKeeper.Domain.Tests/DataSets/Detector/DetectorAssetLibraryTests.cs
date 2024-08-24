using FluentAssertions;
using SightKeeper.Domain.Model.DataSets.Detector;

namespace SightKeeper.Domain.Tests.DataSets.Detector;

public sealed class DetectorAssetLibraryTests
{
	[Fact]
	public void ShouldCreateAsset()
	{
		DetectorDataSet dataSet = new();
		var screenshot = dataSet.Screenshots.CreateScreenshot(DateTime.Now, out _);
		var asset = dataSet.Assets.MakeAsset(screenshot);
		screenshot.Asset.Should().Be(asset);
		dataSet.Assets.Should().Contain(asset);
	}

	[Fact]
	public void ShouldNotCreateDuplicateAsset()
	{
		DetectorDataSet dataSet = new();
		var screenshot = dataSet.Screenshots.CreateScreenshot(DateTime.Now, out _);
		var asset = dataSet.Assets.MakeAsset(screenshot);
		Assert.ThrowsAny<Exception>(() => dataSet.Assets.MakeAsset(screenshot));
		screenshot.Asset.Should().Be(asset);
		dataSet.Assets.Should().Contain(asset);
		dataSet.Assets.Should().HaveCount(1);
	}

	[Fact]
	public void ShouldDeleteAsset()
	{
		DetectorDataSet dataSet = new();
		var screenshot = dataSet.Screenshots.CreateScreenshot(DateTime.Now, out _);
		var asset = dataSet.Assets.MakeAsset(screenshot);
		dataSet.Assets.DeleteAsset(asset);
		dataSet.Assets.Should().BeEmpty();
		screenshot.Asset.Should().BeNull();
	}

	[Fact]
	public void ShouldNotDeleteAssetFromOtherDataSet()
	{
		DetectorDataSet dataSet1 = new();
		DetectorDataSet dataSet2 = new();
		var screenshot = dataSet1.Screenshots.CreateScreenshot(DateTime.Now, out _);
		var asset = dataSet1.Assets.MakeAsset(screenshot);
		Assert.ThrowsAny<Exception>(() => dataSet2.Assets.DeleteAsset(asset));
		asset.Screenshot.Asset.Should().Be(asset);
	}
}