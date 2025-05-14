using FluentAssertions;
using SightKeeper.Domain.DataSets.Detector;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Domain.Tests.DataSets;

public sealed class ItemsAssetTests
{
	[Fact]
	public void ShouldCreateItem()
	{
		var image = Utilities.CreateImage();
		DetectorDataSet dataSet = new();
		var tag = dataSet.TagsLibrary.CreateTag("");
		var asset = dataSet.AssetsLibrary.MakeAsset(image);
		var item = asset.MakeItem(tag);
		asset.Items.Should().Contain(item);
	}

	[Fact]
	public void ShouldCreateMultipleItemsWithSameTag()
	{
		var image = Utilities.CreateImage();
		DetectorDataSet dataSet = new();
		var tag = dataSet.TagsLibrary.CreateTag("");
		var asset = dataSet.AssetsLibrary.MakeAsset(image);
		var item1 = asset.MakeItem(tag);
		var item2 = asset.MakeItem(tag);
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
		var item1 = asset.MakeItem(tag1);
		var item2 = asset.MakeItem(tag2);
		asset.Items.Should().Contain([item1, item2]);
	}

	[Fact]
	public void ShouldNotCreateItemWithWrongOwnerTag()
	{
		var image = Utilities.CreateImage();
		DetectorDataSet dataSet = new();
		var tag2 = new DetectorDataSet().TagsLibrary.CreateTag("2");
		var asset = dataSet.AssetsLibrary.MakeAsset(image);
		var exception = Assert.Throws<UnexpectedTagsOwnerException>(() => asset.MakeItem(tag2));
		tag2.Users.Should().BeEmpty();
		exception.ExpectedOwner.Should().Be(dataSet.TagsLibrary);
		exception.Causer.Should().Be(tag2);
	}

	[Fact]
	public void ShouldDeleteItem()
	{
		var image = Utilities.CreateImage();
		DetectorDataSet dataSet = new();
		var tag = dataSet.TagsLibrary.CreateTag("");
		var asset = dataSet.AssetsLibrary.MakeAsset(image);
		var item = asset.MakeItem(tag);
		asset.DeleteItem(item);
		asset.Items.Should().BeEmpty();
	}

	[Fact]
	public void ShouldNotDeleteItemTwice()
	{
		var image = Utilities.CreateImage();
		DetectorDataSet dataSet = new();
		var tag = dataSet.TagsLibrary.CreateTag("");
		var asset = dataSet.AssetsLibrary.MakeAsset(image);
		var item = asset.MakeItem(tag);
		asset.DeleteItem(item);
		Assert.Throws<ArgumentException>(() => asset.DeleteItem(item));
	}

	[Fact]
	public void ShouldDeleteItemByIndex()
	{
		var image = Utilities.CreateImage();
		DetectorDataSet dataSet = new();
		var tag = dataSet.TagsLibrary.CreateTag("");
		var asset = dataSet.AssetsLibrary.MakeAsset(image);
		asset.MakeItem(tag);
		asset.DeleteItemAt(0);
	}

	[Fact]
	public void ShouldClearItems()
	{
		var image = Utilities.CreateImage();
		DetectorDataSet dataSet = new();
		var tag = dataSet.TagsLibrary.CreateTag("");
		var asset = dataSet.AssetsLibrary.MakeAsset(image);
		asset.MakeItem(tag);
		asset.MakeItem(tag);
		asset.ClearItems();
		asset.Items.Should().BeEmpty();
	}
}