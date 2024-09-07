using FluentAssertions;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.DataSets.Classifier;

namespace SightKeeper.Domain.Tests.DataSets.Classifier;

public sealed class ClassifierAssetLibraryTests
{
	[Fact]
	public void ShouldCreateAsset()
	{
		ClassifierDataSet dataSet = new();
		dataSet.TagsLibrary.CreateTag("");
		var screenshot = dataSet.ScreenshotsLibrary.CreateScreenshot(DateTime.Now, new Vector2<ushort>(320, 320), out _);
		var asset = dataSet.AssetsLibrary.MakeAsset(screenshot);
		screenshot.Asset.Should().Be(asset);
		dataSet.AssetsLibrary.Assets.Should().Contain(asset);
	}

	[Fact]
	public void ShouldNotCreateDuplicateAsset()
	{
		ClassifierDataSet dataSet = new();
		dataSet.TagsLibrary.CreateTag("");
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
		ClassifierDataSet dataSet = new();
		dataSet.TagsLibrary.CreateTag("");
		var screenshot = dataSet.ScreenshotsLibrary.CreateScreenshot(DateTime.Now, new Vector2<ushort>(320, 320), out _);
		var asset = dataSet.AssetsLibrary.MakeAsset(screenshot);
		dataSet.AssetsLibrary.DeleteAsset(asset);
		dataSet.AssetsLibrary.Assets.Should().BeEmpty();
		screenshot.Asset.Should().BeNull();
	}

	[Fact]
	public void ShouldNotDeleteAssetFromOtherDataSet()
	{
		ClassifierDataSet dataSet1 = new();
		dataSet1.TagsLibrary.CreateTag("");
		ClassifierDataSet dataSet2 = new();
		var screenshot = dataSet1.ScreenshotsLibrary.CreateScreenshot(DateTime.Now, new Vector2<ushort>(320, 320), out _);
		var asset = dataSet1.AssetsLibrary.MakeAsset(screenshot);
		Assert.ThrowsAny<Exception>(() => dataSet2.AssetsLibrary.DeleteAsset(asset));
		asset.Screenshot.Asset.Should().Be(asset);
	}
	
	[Fact]
	public void ShouldNotSetAssetTagToForeign()
	{
		ClassifierDataSet dataSet = new();
		var properTag = dataSet.TagsLibrary.CreateTag("");
		var foreignTag = new ClassifierDataSet().TagsLibrary.CreateTag("");
		var screenshot = dataSet.ScreenshotsLibrary.CreateScreenshot(DateTime.Now, new Vector2<ushort>(320, 320), out _);
		var asset = dataSet.AssetsLibrary.MakeAsset(screenshot);
		Assert.ThrowsAny<Exception>(() => asset.Tag = foreignTag);
		asset.Tag.Should().Be(properTag);
	}
	
	[Fact]
	public void ShouldNotCreateAssetWithoutAvailableTags()
	{
		ClassifierDataSet dataSet = new();
		var screenshot = dataSet.ScreenshotsLibrary.CreateScreenshot(DateTime.Now, new Vector2<ushort>(320, 320), out _);
		Assert.ThrowsAny<Exception>(() => dataSet.AssetsLibrary.MakeAsset(screenshot));
	}
}