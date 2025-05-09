using FluentAssertions;
using SightKeeper.Domain.DataSets.Assets.Items;
using SightKeeper.Domain.DataSets.Poser2D;

namespace SightKeeper.Domain.Tests.DataSets.Poser2D;

public class Poser2DTagTests
{
	[Fact]
	public void ShouldAddNewKeyPointTagToPoserTagWithAssociatedItems()
	{
		var image = Utilities.CreateImage();
		Poser2DDataSet dataSet = new();
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
		Poser2DDataSet dataSet = new();
		var tag = dataSet.TagsLibrary.CreateTag("");
		var keyPointTag1 = tag.CreateKeyPointTag("1");
		var asset = dataSet.AssetsLibrary.MakeAsset(image);
		var item = asset.MakeItem(tag, new Bounding());
		var keyPoint = item.CreateKeyPoint(keyPointTag1, new Vector2<double>(0.1, 0.2));
		Assert.ThrowsAny<Exception>(() => tag.DeleteKeyPointTag(keyPointTag1));
		item.KeyPoints.Should().Contain(keyPoint);
	}

	[Fact]
	public void ShouldAddNewPointToTagWithoutAssociatedItems()
	{
		var image = Utilities.CreateImage();
		Poser2DDataSet dataSet = new();
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
		Poser2DDataSet dataSet = new();
		var tag1 = dataSet.TagsLibrary.CreateTag("1");
		var tag2 = dataSet.TagsLibrary.CreateTag("2");
		var keyPoint2 = tag2.CreateKeyPointTag("");
		var asset = dataSet.AssetsLibrary.MakeAsset(image);
		asset.MakeItem(tag1, new Bounding());
		tag2.DeleteKeyPointTag(keyPoint2);
		tag2.KeyPointTags.Should().BeEmpty();
	}
}