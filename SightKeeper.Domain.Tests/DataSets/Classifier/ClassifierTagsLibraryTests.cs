using FluentAssertions;
using SightKeeper.Domain.DataSets.Classifier;
using SightKeeper.Domain.Images;

namespace SightKeeper.Domain.Tests.DataSets.Classifier;

public sealed class ClassifierTagsLibraryTests
{
	[Fact]
	public void ShouldCreateTag()
	{
		ClassifierDataSet dataSet = new();
		var tag = dataSet.TagsLibrary.CreateTag("");
		dataSet.TagsLibrary.Tags.Should().Contain(tag);
	}

	[Fact]
	public void ShouldCreateMultipleTags()
	{
		ClassifierDataSet dataSet = new();
		var tag1 = dataSet.TagsLibrary.CreateTag("1");
		var tag2 = dataSet.TagsLibrary.CreateTag("2");
		var tag3 = dataSet.TagsLibrary.CreateTag("3");
		dataSet.TagsLibrary.Tags.Should().Contain([tag1, tag2, tag3]);
	}

	[Fact]
	public void ShouldNotCreateTagWithOccupiedName()
	{
		ClassifierDataSet dataSet = new();
		var tag1 = dataSet.TagsLibrary.CreateTag("1");
		Assert.ThrowsAny<Exception>(() => dataSet.TagsLibrary.CreateTag("1"));
		dataSet.TagsLibrary.Tags.Should().Contain(tag1);
		dataSet.TagsLibrary.Tags.Should().HaveCount(1);
	}

	[Fact]
	public void ShouldDeleteTag()
	{
		ClassifierDataSet dataSet = new();
		var tag = dataSet.TagsLibrary.CreateTag("");
		dataSet.TagsLibrary.DeleteTag(tag);
		dataSet.TagsLibrary.Tags.Should().BeEmpty();
	}

	[Fact]
	public void ShouldNotDeleteTagWithAssociatedAsset()
	{
		ImageSet imageSet = new();
		var image = imageSet.CreateImage(DateTimeOffset.Now, new Vector2<ushort>(320, 320));
		ClassifierDataSet dataSet = new();
		var tag = dataSet.TagsLibrary.CreateTag("");
		var asset = dataSet.AssetsLibrary.MakeAsset(image);
		Assert.ThrowsAny<Exception>(() => dataSet.TagsLibrary.DeleteTag(tag));
		dataSet.TagsLibrary.Tags.Should().Contain(tag);
		dataSet.AssetsLibrary.Assets.Should().ContainValue(asset);
		asset.Tag.Should().Be(tag);
	}
}