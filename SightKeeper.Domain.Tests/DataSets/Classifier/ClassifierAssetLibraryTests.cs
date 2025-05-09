using FluentAssertions;
using SightKeeper.Domain.DataSets.Classifier;
using SightKeeper.Domain.Images;

namespace SightKeeper.Domain.Tests.DataSets.Classifier;

public sealed class ClassifierAssetLibraryTests
{
	[Fact]
	public void ShouldCreateAsset()
	{
		var image = Utilities.CreateImage();
		ClassifierDataSet dataSet = new();
		dataSet.TagsLibrary.CreateTag("");
		var asset = dataSet.AssetsLibrary.MakeAsset(image);
		dataSet.AssetsLibrary.Assets.Should().ContainKey(image).WhoseValue.Should().Be(asset);
	}

	[Fact]
	public void ShouldNotCreateAssetForSameImageTwice()
	{
		var image = Utilities.CreateImage();
		ClassifierDataSet dataSet = new();
		dataSet.TagsLibrary.CreateTag("");
		var asset = dataSet.AssetsLibrary.MakeAsset(image);
		Assert.Throws<ArgumentException>(() => dataSet.AssetsLibrary.MakeAsset(image));
		dataSet.AssetsLibrary.Assets.Should().ContainValue(asset).And.HaveCount(1);
	}

	[Fact]
	public void ShouldDeleteAsset()
	{
		var image = Utilities.CreateImage();
		ClassifierDataSet dataSet = new();
		dataSet.TagsLibrary.CreateTag("");
		dataSet.AssetsLibrary.MakeAsset(image);
		dataSet.AssetsLibrary.DeleteAsset(image);
		dataSet.AssetsLibrary.Assets.Should().BeEmpty();
	}

	[Fact]
	public void ShouldNotCreateAssetWithoutAvailableTags()
	{
		ImageSet imageSet = new();
		var image = imageSet.CreateImage(DateTimeOffset.Now, new Vector2<ushort>(320, 320));
		ClassifierDataSet dataSet = new();
		Assert.Throws<ArgumentOutOfRangeException>(() => dataSet.AssetsLibrary.MakeAsset(image));
	}
}