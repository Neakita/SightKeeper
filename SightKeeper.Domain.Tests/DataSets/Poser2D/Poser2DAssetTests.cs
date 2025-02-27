using FluentAssertions;
using SightKeeper.Domain.DataSets.Assets.Items;
using SightKeeper.Domain.DataSets.Poser2D;
using SightKeeper.Domain.Images;

namespace SightKeeper.Domain.Tests.DataSets.Poser2D;

public sealed class Poser2DAssetTests
{
	[Fact]
	public void ShouldCreateItem()
	{
		ImageSet imageSet = new();
		var screenshot = imageSet.CreateImage(DateTimeOffset.Now, new Vector2<ushort>(320, 320));
		Poser2DDataSet dataSet = new();
		var tag = dataSet.TagsLibrary.CreateTag("");
		var asset = dataSet.AssetsLibrary.MakeAsset(screenshot);
		var item = asset.CreateItem(tag, new Bounding());
		asset.Items.Should().Contain(item);
	}

	[Fact]
	public void ShouldCreateMultipleItemsWithSameTag()
	{
		ImageSet imageSet = new();
		var screenshot = imageSet.CreateImage(DateTimeOffset.Now, new Vector2<ushort>(320, 320));
		Poser2DDataSet dataSet = new();
		var tag = dataSet.TagsLibrary.CreateTag("");
		var asset = dataSet.AssetsLibrary.MakeAsset(screenshot);
		var item1 = asset.CreateItem(tag, new Bounding(0, 0, 0.5, 0.5));
		var item2 = asset.CreateItem(tag, new Bounding(0, 0, 1, 1));
		asset.Items.Should().Contain([item1, item2]);
	}

	[Fact]
	public void ShouldCreateMultipleItemsWithSameBounding()
	{
		ImageSet imageSet = new();
		var screenshot = imageSet.CreateImage(DateTimeOffset.Now, new Vector2<ushort>(320, 320));
		Poser2DDataSet dataSet = new();
		var tag1 = dataSet.TagsLibrary.CreateTag("1");
		var tag2 = dataSet.TagsLibrary.CreateTag("2");
		var asset = dataSet.AssetsLibrary.MakeAsset(screenshot);
		var item1 = asset.CreateItem(tag1, new Bounding());
		var item2 = asset.CreateItem(tag2, new Bounding());
		asset.Items.Should().Contain([item1, item2]);
	}

	[Fact]
	public void ShouldCreateKeyPoints()
	{
		ImageSet imageSet = new();
		var screenshot = imageSet.CreateImage(DateTimeOffset.Now, new Vector2<ushort>(320, 320));
		Poser2DDataSet dataSet = new();
		var tag = dataSet.TagsLibrary.CreateTag("");
		var firstKeyPointTag = tag.CreateKeyPointTag("1");
		var secondKeyPointTag = tag.CreateKeyPointTag("2");
		var asset = dataSet.AssetsLibrary.MakeAsset(screenshot);
		var item = asset.CreateItem(tag, new Bounding());
		var firstKeyPoint = item.CreateKeyPoint(firstKeyPointTag, new Vector2<double>(0.1, 0.2));
		var secondKeyPoint = item.CreateKeyPoint(secondKeyPointTag, new Vector2<double>(0.3, 0.4));
		asset.Items.Should().Contain(item);
		item.KeyPoints.Should().Contain(firstKeyPoint).And.Contain(secondKeyPoint);
	}
}