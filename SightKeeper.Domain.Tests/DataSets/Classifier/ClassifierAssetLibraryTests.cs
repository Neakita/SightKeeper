using FluentAssertions;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.DataSets.Classifier;
using SightKeeper.Domain.Model.DataSets.Screenshots;

namespace SightKeeper.Domain.Tests.DataSets.Classifier;

public sealed class ClassifierAssetLibraryTests
{
	[Fact]
	public void ShouldCreateAsset()
	{
		ScreenshotsLibrary screenshotsLibrary = new();
		var screenshot = screenshotsLibrary.CreateScreenshot(DateTimeOffset.Now, new Vector2<ushort>(320, 320));
		ClassifierDataSet dataSet = new();
		dataSet.TagsLibrary.CreateTag("");
		var asset = dataSet.AssetsLibrary.MakeAsset(screenshot);
		dataSet.AssetsLibrary.Assets.Should().ContainKey(screenshot).WhoseValue.Should().Be(asset);
	}

	[Fact]
	public void ShouldNotCreateDuplicateAsset()
	{
		ScreenshotsLibrary screenshotsLibrary = new();
		var screenshot = screenshotsLibrary.CreateScreenshot(DateTimeOffset.Now, new Vector2<ushort>(320, 320));
		ClassifierDataSet dataSet = new();
		dataSet.TagsLibrary.CreateTag("");
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
		ClassifierDataSet dataSet = new();
		dataSet.TagsLibrary.CreateTag("");
		dataSet.AssetsLibrary.MakeAsset(screenshot);
		dataSet.AssetsLibrary.DeleteAsset(screenshot);
		dataSet.AssetsLibrary.Assets.Should().BeEmpty();
	}

	[Fact]
	public void ShouldNotDeleteAssetFromOtherDataSet()
	{
		ScreenshotsLibrary screenshotsLibrary = new();
		var screenshot = screenshotsLibrary.CreateScreenshot(DateTimeOffset.Now, new Vector2<ushort>(320, 320));
		ClassifierDataSet dataSet1 = new();
		dataSet1.TagsLibrary.CreateTag("");
		ClassifierDataSet dataSet2 = new();
		var asset = dataSet1.AssetsLibrary.MakeAsset(screenshot);
		Assert.ThrowsAny<Exception>(() => dataSet2.AssetsLibrary.DeleteAsset(screenshot));
		dataSet1.AssetsLibrary.Assets.Should().ContainKey(screenshot).WhoseValue.Should().Be(asset);
	}
	
	[Fact]
	public void ShouldNotSetAssetTagToForeign()
	{
		ScreenshotsLibrary screenshotsLibrary = new();
		var screenshot = screenshotsLibrary.CreateScreenshot(DateTimeOffset.Now, new Vector2<ushort>(320, 320));
		ClassifierDataSet dataSet = new();
		var properTag = dataSet.TagsLibrary.CreateTag("");
		var foreignTag = new ClassifierDataSet().TagsLibrary.CreateTag("");
		var asset = dataSet.AssetsLibrary.MakeAsset(screenshot);
		Assert.ThrowsAny<Exception>(() => asset.Tag = foreignTag);
		asset.Tag.Should().Be(properTag);
	}
	
	[Fact]
	public void ShouldNotCreateAssetWithoutAvailableTags()
	{
		ScreenshotsLibrary screenshotsLibrary = new();
		var screenshot = screenshotsLibrary.CreateScreenshot(DateTimeOffset.Now, new Vector2<ushort>(320, 320));
		ClassifierDataSet dataSet = new();
		Assert.ThrowsAny<Exception>(() => dataSet.AssetsLibrary.MakeAsset(screenshot));
	}
}