using FluentAssertions;
using SightKeeper.Domain.DataSets.Detector;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Domain.Tests.DataSets.Detector;

public sealed class DetectorTagsLibraryTests
{
	[Fact]
	public void ShouldCreateTag()
	{
		DomainDetectorDataSet dataSet = new();
		var tag = dataSet.TagsLibrary.CreateTag("");
		dataSet.TagsLibrary.Tags.Should().Contain(tag);
	}

	[Fact]
	public void ShouldCreateMultipleTags()
	{
		DomainDetectorDataSet dataSet = new();
		var tag1 = dataSet.TagsLibrary.CreateTag("1");
		var tag2 = dataSet.TagsLibrary.CreateTag("2");
		var tag3 = dataSet.TagsLibrary.CreateTag("3");
		dataSet.TagsLibrary.Tags.Should().Contain([tag1, tag2, tag3]);
	}

	[Fact]
	public void ShouldNotCreateTagWithOccupiedName()
	{
		DomainDetectorDataSet dataSet = new();
		var tag1 = dataSet.TagsLibrary.CreateTag("1");
		Assert.ThrowsAny<Exception>(() => dataSet.TagsLibrary.CreateTag("1"));
		dataSet.TagsLibrary.Tags.Should().Contain(tag1);
		dataSet.TagsLibrary.Tags.Should().HaveCount(1);
	}

	[Fact]
	public void ShouldDeleteTag()
	{
		DomainDetectorDataSet dataSet = new();
		var tag = dataSet.TagsLibrary.CreateTag("");
		dataSet.TagsLibrary.DeleteTag(tag);
		dataSet.TagsLibrary.Tags.Should().BeEmpty();
	}

	[Fact]
	public void ShouldNotDeleteTagWithItems()
	{
		var image = Utilities.CreateImage();
		DomainDetectorDataSet dataSet = new();
		var tag = dataSet.TagsLibrary.CreateTag("");
		var asset = dataSet.AssetsLibrary.MakeAsset(image);
		asset.MakeItem(tag);
		var exception = Assert.Throws<TagIsInUseException>(() => dataSet.TagsLibrary.DeleteTag(tag));
		dataSet.TagsLibrary.Tags.Should().Contain(tag);
		exception.Tag.Should().Be(tag);
	}

	[Fact]
	public void ShouldDeleteTagWithoutItems()
	{
		var image = Utilities.CreateImage();
		DomainDetectorDataSet dataSet = new();
		var tag1 = dataSet.TagsLibrary.CreateTag("1");
		var tag2 = dataSet.TagsLibrary.CreateTag("2");
		var asset = dataSet.AssetsLibrary.MakeAsset(image);
		asset.MakeItem(tag1);
		dataSet.TagsLibrary.DeleteTag(tag2);
		dataSet.TagsLibrary.Tags.Should().Contain(tag1);
		dataSet.TagsLibrary.Tags.Should().NotContain(tag2);
	}
}