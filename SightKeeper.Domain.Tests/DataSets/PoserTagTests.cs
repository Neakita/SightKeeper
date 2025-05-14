using FluentAssertions;
using SightKeeper.Domain.DataSets.Assets.Items;
using SightKeeper.Domain.DataSets.Poser;
using SightKeeper.Domain.DataSets.Poser2D;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Domain.Tests.DataSets;

public sealed class PoserTagTests
{
	[Fact]
	public void ShouldAddNewKeyPointTagToPoserTagWithAssociatedItems()
	{
		var image = Utilities.CreateImage();
		var dataSet = CreateDataSet();
		var tag = dataSet.TagsLibrary.CreateTag("");
		var asset = dataSet.AssetsLibrary.MakeAsset(image);
		asset.MakeItem(tag, new Bounding());
		var keyPointTag = tag.CreateKeyPointTag("");
		tag.KeyPointTags.Should().Contain(keyPointTag);
	}

	[Fact]
	public void ShouldNotDeleteKeyPointTagWithAssociatedKeyPoint()
	{
		var image = Utilities.CreateImage();
		var dataSet = CreateDataSet();
		var tag = dataSet.TagsLibrary.CreateTag("");
		var keyPointTag1 = tag.CreateKeyPointTag("1");
		var asset = dataSet.AssetsLibrary.MakeAsset(image);
		var item = asset.MakeItem(tag, new Bounding());
		var keyPoint = item.MakeKeyPoint(keyPointTag1);
		Assert.ThrowsAny<Exception>(() => tag.DeleteKeyPointTag(keyPointTag1));
		item.KeyPoints.Should().Contain(keyPoint);
	}

	[Fact]
	public void ShouldAddNewPointToTagWithoutAssociatedItems()
	{
		var image = Utilities.CreateImage();
		var dataSet = CreateDataSet();
		var tag1 = dataSet.TagsLibrary.CreateTag("1");
		var tag2 = dataSet.TagsLibrary.CreateTag("2");
		var asset = dataSet.AssetsLibrary.MakeAsset(image);
		asset.MakeItem(tag1, new Bounding());
		var keyPoint = tag2.CreateKeyPointTag("");
		tag2.KeyPointTags.Should().Contain(keyPoint);
	}

	[Fact]
	public void ShouldDeletePointOfTagWithoutAssociatedItems()
	{
		var image = Utilities.CreateImage();
		var dataSet = CreateDataSet();
		var tag1 = dataSet.TagsLibrary.CreateTag("1");
		var tag2 = dataSet.TagsLibrary.CreateTag("2");
		var keyPoint2 = tag2.CreateKeyPointTag("");
		var asset = dataSet.AssetsLibrary.MakeAsset(image);
		asset.MakeItem(tag1, new Bounding());
		tag2.DeleteKeyPointTag(keyPoint2);
		tag2.KeyPointTags.Should().BeEmpty();
	}

	[Fact]
	public void ShouldNotDeleteKeyPointTagTwice()
	{
		var dataSet = CreateDataSet();
		var tag = dataSet.TagsLibrary.CreateTag("");
		var keyPointTag = tag.CreateKeyPointTag("");
		tag.DeleteKeyPointTag(keyPointTag);
		Assert.Throws<ArgumentException>(() => tag.DeleteKeyPointTag(keyPointTag));
	}

	[Fact]
	public void ShouldDeleteKeyPointTagByIndex()
	{
		var dataSet = CreateDataSet();
		var tag = dataSet.TagsLibrary.CreateTag("");
		tag.CreateKeyPointTag("");
		tag.DeleteKeyPointTagAt(0);
		tag.KeyPointTags.Should().BeEmpty();
	}

	[Fact]
	public void ShouldCreateKeyPointTagViaTagsOwner()
	{
		var dataSet = CreateDataSet();
		var tag = dataSet.TagsLibrary.CreateTag("");
		TagsOwner<Tag> tagAsTagsOwner = tag;
		var keyPointTag = tagAsTagsOwner.CreateTag("");
		tag.KeyPointTags.Should().Contain(keyPointTag);
	}

	[Fact]
	public void ShouldDeleteKeyPointTagByIndexViaTagsOwner()
	{
		var dataSet = CreateDataSet();
		var tag = dataSet.TagsLibrary.CreateTag("");
		tag.CreateKeyPointTag("");
		TagsOwner<Tag> tagAsTagsOwner = tag;
		tagAsTagsOwner.DeleteTagAt(0);
		tag.KeyPointTags.Should().BeEmpty();
	}

	private static PoserDataSet CreateDataSet()
	{
		return new Poser2DDataSet();
	}
}