using FluentAssertions;
using SightKeeper.Domain.DataSets.Assets.Items;
using SightKeeper.Domain.DataSets.Detector;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Domain.Tests.DataSets.Detector;

public sealed class DetectorAssetTests
{
	[Fact]
	public void ShouldCreateItem()
	{
		var image = Utilities.CreateImage();
		DetectorDataSet dataSet = new();
		var tag = dataSet.TagsLibrary.CreateTag("");
		var asset = dataSet.AssetsLibrary.MakeAsset(image);
		var item = asset.MakeItem(tag, new Bounding());
		asset.Items.Should().Contain(item);
	}

	[Fact]
	public void ShouldCreateMultipleItemsWithSameTag()
	{
		var image = Utilities.CreateImage();
		DetectorDataSet dataSet = new();
		var tag = dataSet.TagsLibrary.CreateTag("");
		var asset = dataSet.AssetsLibrary.MakeAsset(image);
		var item1 = asset.MakeItem(tag, new Bounding(0, 0, 0.5, 0.5));
		var item2 = asset.MakeItem(tag, new Bounding(0, 0, 1, 1));
		asset.Items.Should().Contain([item1, item2]);
	}

	[Fact]
	public void ShouldCreateMultipleItemsWithSameBounding()
	{
		var image = Utilities.CreateImage();
		DetectorDataSet dataSet = new();
		var tag1 = dataSet.TagsLibrary.CreateTag("1");
		var tag2 = dataSet.TagsLibrary.CreateTag("2");
		var asset = dataSet.AssetsLibrary.MakeAsset(image);
		var item1 = asset.MakeItem(tag1, new Bounding());
		var item2 = asset.MakeItem(tag2, new Bounding());
		asset.Items.Should().Contain([item1, item2]);
	}

	[Fact]
	public void ShouldNotCreateItemWithWrongOwnerTag()
	{
		var image = Utilities.CreateImage();
		DetectorDataSet dataSet = new();
		var tag2 = new DetectorDataSet().TagsLibrary.CreateTag("2");
		var asset = dataSet.AssetsLibrary.MakeAsset(image);
		var exception = Assert.Throws<UnexpectedTagsOwnerException>(() => asset.MakeItem(tag2, new Bounding(.1, .2, .3, .4)));
		tag2.Users.Should().BeEmpty();
		exception.ExpectedOwner.Should().Be(dataSet.TagsLibrary);
		exception.Causer.Should().Be(tag2);
	}

	[Fact]
	public void ShouldCreateItemViaItemsCreator()
	{
		var image = Utilities.CreateImage();
		DetectorDataSet dataSet = new();
		var tag = dataSet.TagsLibrary.CreateTag("");
		var asset = dataSet.AssetsLibrary.MakeAsset(image);
		ItemsMaker assetAsItemsMaker = asset;
		var item = (DetectorItem)assetAsItemsMaker.MakeItem(tag, new Bounding());
		asset.Items.Should().Contain(item);
	}
}