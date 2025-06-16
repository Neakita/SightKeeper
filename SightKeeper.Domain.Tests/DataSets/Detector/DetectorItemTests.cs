/*using FluentAssertions;
using SightKeeper.Domain.DataSets.Detector;

namespace SightKeeper.Domain.Tests.DataSets.Detector;

public sealed class DetectorItemTests
{
	[Fact]
	public void ShouldUpdateTag()
	{
		DomainDetectorDataSet dataSet = new();
		var tag1 = dataSet.TagsLibrary.CreateTag("1");
		var tag2 = dataSet.TagsLibrary.CreateTag("2");
		var image = Utilities.CreateImage();
		var asset = dataSet.AssetsLibrary.MakeAsset(image);
		var item = asset.MakeItem(tag1);
		item.Tag = tag2;
		item.Tag.Should().Be(tag2);
		tag1.Users.Should().BeEmpty();
		tag2.Users.Should().Contain(item);
	}
}*/