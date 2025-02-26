using FluentAssertions;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Poser2D;
using SightKeeper.Domain.Images;

namespace SightKeeper.Domain.Tests.DataSets.Poser2D;

public class Poser2DTagTests
{
	[Fact]
	public void ShouldNotChangeTagNameToOccupied()
	{
		Poser2DDataSet dataSet = new();
		var tag1 = dataSet.TagsLibrary.CreateTag("1");
		var tag2 = dataSet.TagsLibrary.CreateTag("2");
		Assert.ThrowsAny<Exception>(() => tag2.Name = "1");
		tag1.Name.Should().Be("1");
		tag2.Name.Should().Be("2");
	}

	[Fact]
	public void ShouldSetTagNameToDeletedTagName()
	{
		Poser2DDataSet dataSet = new();
		var tag1 = dataSet.TagsLibrary.CreateTag("1");
		var tag2 = dataSet.TagsLibrary.CreateTag("2");
		dataSet.TagsLibrary.DeleteTag(tag1);
		tag2.Name = tag1.Name;
	}

	[Fact]
	public void ShouldAddNewKeyPointTagToPoserTagWithAssociatedItems()
	{
		ImageSet imageSet = new();
		var screenshot = imageSet.CreateImage(DateTimeOffset.Now, new Vector2<ushort>(320, 320));
		Poser2DDataSet dataSet = new();
		var tag = dataSet.TagsLibrary.CreateTag("");
		var asset = dataSet.AssetsLibrary.MakeAsset(screenshot);
		asset.CreateItem(tag, new Bounding());
		var keyPointTag = tag.CreateKeyPointTag("");
		tag.KeyPointTags.Should().Contain(keyPointTag);
	}

	[Fact]
	public void ShouldNotDeleteKeyPointTagWithAssociatedKeyPoint()
	{
		ImageSet imageSet = new();
		var screenshot = imageSet.CreateImage(DateTimeOffset.Now, new Vector2<ushort>(320, 320));
		Poser2DDataSet dataSet = new();
		var tag = dataSet.TagsLibrary.CreateTag("");
		var keyPointTag1 = tag.CreateKeyPointTag("1");
		var asset = dataSet.AssetsLibrary.MakeAsset(screenshot);
		var item = asset.CreateItem(tag, new Bounding());
		var keyPoint = item.CreateKeyPoint(keyPointTag1, new Vector2<double>(0.1, 0.2));
		Assert.ThrowsAny<Exception>(() => tag.DeleteKeyPointTag(keyPointTag1));
		item.KeyPoints.Should().Contain(keyPoint);
	}

	[Fact]
	public void ShouldAddNewPointToTagWithoutAssociatedItems()
	{
		ImageSet imageSet = new();
		var screenshot = imageSet.CreateImage(DateTimeOffset.Now, new Vector2<ushort>(320, 320));
		Poser2DDataSet dataSet = new();
		var tag1 = dataSet.TagsLibrary.CreateTag("1");
		var tag2 = dataSet.TagsLibrary.CreateTag("2");
		var asset = dataSet.AssetsLibrary.MakeAsset(screenshot);
		asset.CreateItem(tag1, new Bounding());
		var keyPoint = tag2.CreateKeyPointTag("");
		tag2.KeyPointTags.Should().Contain(keyPoint);
	}

	[Fact]
	public void ShouldDeletePointOfTagWithoutAssociatedItems()
	{
		ImageSet imageSet = new();
		var screenshot = imageSet.CreateImage(DateTimeOffset.Now, new Vector2<ushort>(320, 320));
		Poser2DDataSet dataSet = new();
		var tag1 = dataSet.TagsLibrary.CreateTag("1");
		var tag2 = dataSet.TagsLibrary.CreateTag("2");
		var keyPoint2 = tag2.CreateKeyPointTag("");
		var asset = dataSet.AssetsLibrary.MakeAsset(screenshot);
		asset.CreateItem(tag1, new Bounding());
		tag2.DeleteKeyPointTag(keyPoint2);
		tag2.KeyPointTags.Should().BeEmpty();
	}
}