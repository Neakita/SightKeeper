using FluentAssertions;
using SightKeeper.Domain.DataSets.Assets.Items;
using SightKeeper.Domain.DataSets.Detector;

namespace SightKeeper.Domain.Tests.DataSets;

public sealed class BoundedItemTests
{
	[Fact]
	public void ShouldSetBounding()
	{
		var item = CreateItem();
		item.Bounding = new Bounding(.1, .2, .3, .4);
	}

	[Fact]
	public void ShouldNotSetBoundingToNonNormalized()
	{
		var item = CreateItem();
		Bounding nonNormalizedBounding = new(0.1, 0.2, 1.3, 1.4);
		var exception = Assert.Throws<ItemBoundingConstraintException>(() => item.Bounding = nonNormalizedBounding);
		item.Bounding.Should().NotBe(nonNormalizedBounding);
		exception.Item.Should().Be(item);
		exception.Value.Should().Be(nonNormalizedBounding);
	}

	private static DomainBoundedItem CreateItem()
	{
		DomainDetectorDataSet dataSet = new();
		var image = Utilities.CreateImage();
		var asset = dataSet.AssetsLibrary.MakeAsset(image);
		var tag = dataSet.TagsLibrary.CreateTag(string.Empty);
		return asset.MakeItem(tag);
	}
}