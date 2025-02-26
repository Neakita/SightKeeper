using FluentAssertions;
using SightKeeper.Domain.DataSets.Classifier;
using SightKeeper.Domain.Images;

namespace SightKeeper.Domain.Tests.DataSets.Classifier;

public sealed class ClassifierAssetLibraryTests
{
	[Fact]
	public void ShouldCreateAsset()
	{
		ImageSet imageSet = new();
		var screenshot = imageSet.CreateScreenshot(DateTimeOffset.Now, new Vector2<ushort>(320, 320));
		ClassifierDataSet dataSet = new();
		dataSet.TagsLibrary.CreateTag("");
		var asset = dataSet.AssetsLibrary.MakeAsset(screenshot);
		dataSet.AssetsLibrary.Assets.Should().ContainKey(screenshot).WhoseValue.Should().Be(asset);
	}

	[Fact]
	public void ShouldNotCreateDuplicateAsset()
	{
		ImageSet imageSet = new();
		var screenshot = imageSet.CreateScreenshot(DateTimeOffset.Now, new Vector2<ushort>(320, 320));
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
		ImageSet imageSet = new();
		var screenshot = imageSet.CreateScreenshot(DateTimeOffset.Now, new Vector2<ushort>(320, 320));
		ClassifierDataSet dataSet = new();
		dataSet.TagsLibrary.CreateTag("");
		dataSet.AssetsLibrary.MakeAsset(screenshot);
		dataSet.AssetsLibrary.DeleteAsset(screenshot);
		dataSet.AssetsLibrary.Assets.Should().BeEmpty();
	}

	[Fact]
	public void ShouldNotDeleteAssetFromOtherDataSet()
	{
		ImageSet imageSet = new();
		var screenshot = imageSet.CreateScreenshot(DateTimeOffset.Now, new Vector2<ushort>(320, 320));
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
		ImageSet imageSet = new();
		var screenshot = imageSet.CreateScreenshot(DateTimeOffset.Now, new Vector2<ushort>(320, 320));
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
		ImageSet imageSet = new();
		var screenshot = imageSet.CreateScreenshot(DateTimeOffset.Now, new Vector2<ushort>(320, 320));
		ClassifierDataSet dataSet = new();
		Assert.ThrowsAny<Exception>(() => dataSet.AssetsLibrary.MakeAsset(screenshot));
	}
}