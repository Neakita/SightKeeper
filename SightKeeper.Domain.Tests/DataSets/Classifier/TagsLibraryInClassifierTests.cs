using FluentAssertions;
using SightKeeper.Domain.DataSets.Classifier;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Domain.Tests.DataSets.Classifier;

public sealed class TagsLibraryInClassifierTests
{
	[Fact]
	public void ShouldNotDeleteTagWithAssociatedAsset()
	{
		var image = Utilities.CreateImage();
		DomainClassifierDataSet dataSet = new();
		var tag = dataSet.TagsLibrary.CreateTag("");
		var asset = dataSet.AssetsLibrary.MakeAsset(image);
		var exception = Assert.Throws<TagIsInUseException>(() => dataSet.TagsLibrary.DeleteTag(tag));
		dataSet.TagsLibrary.Tags.Should().Contain(tag);
		dataSet.AssetsLibrary.Assets.Should().Contain(asset);
		dataSet.AssetsLibrary.Images.Should().Contain(image);
		asset.Tag.Should().Be(tag);
		exception.Tag.Should().Be(tag);
	}
}