using FluentAssertions;
using SightKeeper.Domain.DataSets.Classifier;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Domain.Tests.DataSets.Classifier;

public sealed class ClassifierAssetTests
{
	[Fact]
	public void ShouldUpdateTag()
	{
		var image = Utilities.CreateImage();
		ClassifierDataSet dataSet = new();
		var tag1 = dataSet.TagsLibrary.CreateTag("1");
		var asset = dataSet.AssetsLibrary.MakeAsset(image);
		var tag2 = dataSet.TagsLibrary.CreateTag("2");
		asset.Tag = tag2;
		asset.Tag.Should().Be(tag2);
		tag1.Users.Should().BeEmpty();
		tag2.Users.Should().Contain(asset);
	}

	[Fact]
	public void ShouldNotSetTagToForeign()
	{
		var image = Utilities.CreateImage();
		ClassifierDataSet dataSet = new();
		var properTag = dataSet.TagsLibrary.CreateTag("");
		var foreignTag = new ClassifierDataSet().TagsLibrary.CreateTag("");
		var asset = dataSet.AssetsLibrary.MakeAsset(image);
		var exception = Assert.Throws<InappropriateTagsOwnerChangeException>(() => asset.Tag = foreignTag);
		asset.Tag.Should().Be(properTag);
		exception.Causer.Should().Be(foreignTag);
		exception.CurrentTag.Should().Be(properTag);
	}
}