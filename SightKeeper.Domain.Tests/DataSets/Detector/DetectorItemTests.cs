using FluentAssertions;
using SightKeeper.Domain.DataSets.Assets.Items;
using SightKeeper.Domain.DataSets.Detector;
using SightKeeper.Domain.Images;

namespace SightKeeper.Domain.Tests.DataSets.Detector;

public sealed class DetectorItemTests
{
	[Fact]
	public void ShouldUpdateTag()
	{
		DetectorDataSet dataSet = new();
		var tag1 = dataSet.TagsLibrary.CreateTag("1");
		var tag2 = dataSet.TagsLibrary.CreateTag("2");
		ImageSet imageSet = new();
		var image = imageSet.CreateImage(DateTimeOffset.UtcNow, new Vector2<ushort>(320, 320));
		var asset = dataSet.AssetsLibrary.MakeAsset(image);
		var item = asset.MakeItem(tag1, new Bounding(.1, .2, .3, .4));
		item.Tag = tag2;
		item.Tag.Should().Be(tag2);
		tag1.Users.Should().BeEmpty();
		tag2.Users.Should().Contain(item);
	}
}