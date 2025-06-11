using FluentAssertions;
using SightKeeper.Domain.DataSets.Classifier;

namespace SightKeeper.Domain.Tests.DataSets.Classifier;

public sealed class ClassifierAssetLibraryTests
{
	[Fact]
	public void ShouldCreateAssetWithInitialTag()
	{
		var image = Utilities.CreateImage();
		DomainClassifierDataSet dataSet = new();
		var tag = dataSet.TagsLibrary.CreateTag("");
		var asset = dataSet.AssetsLibrary.MakeAsset(image);
		dataSet.AssetsLibrary.Assets.Should().Contain(asset);
		dataSet.AssetsLibrary.Images.Should().Contain(image);
		asset.Image.Should().Be(image);
		asset.Tag.Should().Be(tag);
	}

	[Fact]
	public void ShouldNotCreateAssetWithoutAvailableTags()
	{
		var image = Utilities.CreateImage();
		DomainClassifierDataSet dataSet = new();
		Assert.Throws<ArgumentOutOfRangeException>(() => dataSet.AssetsLibrary.MakeAsset(image));
	}
}