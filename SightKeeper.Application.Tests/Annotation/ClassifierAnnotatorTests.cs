using FluentAssertions;
using SightKeeper.Application.Annotation;
using SightKeeper.Domain;
using SightKeeper.Domain.DataSets.Classifier;
using SightKeeper.Domain.Images;

namespace SightKeeper.Application.Tests.Annotation;

public sealed class ClassifierAnnotatorTests
{
	[Fact]
	public void ShouldCreateAssetAndSetTag()
	{
		DomainImageSet imageSet = new();
		var image = imageSet.CreateImage(DateTimeOffset.UtcNow, new Vector2<ushort>(320, 320));
		DomainClassifierDataSet dataSet = new();
		var tag = dataSet.TagsLibrary.CreateTag("");
		ClassifierAnnotator annotator = new(new FakeAssetsMaker());
		annotator.SetTag(dataSet.AssetsLibrary, image, tag);
		dataSet.AssetsLibrary.Assets.Should().Contain(asset => asset.Image == image && asset.Tag == tag);
	}

	[Fact]
	public void ShouldSetTagInExistingAsset()
	{
		DomainImageSet imageSet = new();
		var image = imageSet.CreateImage(DateTimeOffset.UtcNow, new Vector2<ushort>(320, 320));
		DomainClassifierDataSet dataSet = new();
		dataSet.TagsLibrary.CreateTag("1");
		var tag2 = dataSet.TagsLibrary.CreateTag("2");
		var asset = dataSet.AssetsLibrary.MakeAsset(image);
		ClassifierAnnotator annotator = new(new FakeAssetsMaker());
		annotator.SetTag(dataSet.AssetsLibrary, image, tag2);
		asset.Tag.Should().Be(tag2);
	}

	[Fact]
	public void ShouldDeleteAsset()
	{
		DomainImageSet imageSet = new();
		var image = imageSet.CreateImage(DateTimeOffset.UtcNow, new Vector2<ushort>(320, 320));
		DomainClassifierDataSet dataSet = new();
		dataSet.TagsLibrary.CreateTag("1");
		var asset = dataSet.AssetsLibrary.MakeAsset(image);
		ClassifierAnnotator annotator = new(new FakeAssetsMaker());
		annotator.DeleteAsset(dataSet.AssetsLibrary, image);
		dataSet.AssetsLibrary.Assets.Should().NotContain(asset);
	}

	[Fact]
	public void ShouldNotDeleteAssetThatDoesNotExist()
	{
		DomainImageSet imageSet = new();
		var image = imageSet.CreateImage(DateTimeOffset.UtcNow, new Vector2<ushort>(320, 320));
		DomainClassifierDataSet dataSet = new();
		dataSet.TagsLibrary.CreateTag("1");
		ClassifierAnnotator annotator = new(new FakeAssetsMaker());
		Assert.Throws<ArgumentException>(() => annotator.DeleteAsset(dataSet.AssetsLibrary, image));
	}
}