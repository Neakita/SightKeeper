using FluentAssertions;
using SightKeeper.Domain.DataSets.Assets.Items;
using SightKeeper.Domain.DataSets.Poser2D;

namespace SightKeeper.Domain.Tests.DataSets.Poser2D;

public sealed class Poser2DAssetTests
{
	[Fact]
	public void ShouldCreateKeyPoints()
	{
		var image = Utilities.CreateImage();
		Poser2DDataSet dataSet = new();
		var tag = dataSet.TagsLibrary.CreateTag("");
		var firstKeyPointTag = tag.CreateKeyPointTag("1");
		var secondKeyPointTag = tag.CreateKeyPointTag("2");
		var asset = dataSet.AssetsLibrary.MakeAsset(image);
		var item = asset.MakeItem(tag, new Bounding());
		var firstKeyPoint = item.MakeKeyPoint(firstKeyPointTag, new Vector2<double>(0.1, 0.2));
		var secondKeyPoint = item.MakeKeyPoint(secondKeyPointTag, new Vector2<double>(0.3, 0.4));
		asset.Items.Should().Contain(item);
		item.KeyPoints.Should().Contain(firstKeyPoint).And.Contain(secondKeyPoint);
	}

	[Fact]
	public void ShouldDeleteKeyPoint()
	{
		var image = Utilities.CreateImage();
		Poser2DDataSet dataSet = new();
		var tag = dataSet.TagsLibrary.CreateTag("");
		var keyPointTag = tag.CreateKeyPointTag("1");
		var asset = dataSet.AssetsLibrary.MakeAsset(image);
		var item = asset.MakeItem(tag, new Bounding());
		var keyPoint = item.MakeKeyPoint(keyPointTag, new Vector2<double>(0.1, 0.2));
		item.DeleteKeyPoint(keyPoint);
		item.KeyPoints.Should().NotContain(keyPoint);
	}

	[Fact]
	public void ShouldNotDeleteKeyPointTwice()
	{
		var image = Utilities.CreateImage();
		Poser2DDataSet dataSet = new();
		var tag = dataSet.TagsLibrary.CreateTag("");
		var keyPointTag = tag.CreateKeyPointTag("1");
		var asset = dataSet.AssetsLibrary.MakeAsset(image);
		var item = asset.MakeItem(tag, new Bounding());
		var keyPoint = item.MakeKeyPoint(keyPointTag, new Vector2<double>(0.1, 0.2));
		item.DeleteKeyPoint(keyPoint);
		Assert.Throws<ArgumentException>(() => item.DeleteKeyPoint(keyPoint));
	}
}