using FluentAssertions;
using NSubstitute;
using SightKeeper.Data.DataSets.Classifier.Decorators;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Classifier;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.DataSets.Weights;

namespace SightKeeper.Data.Tests.DataSets.Classifier;

public sealed class OverrideLibrariesClassifierDataSetTests
{
	[Fact]
	public void ShouldOverrideTagsLibrary()
	{
		var innerSet = Substitute.For<ClassifierDataSet>();
		var tagsLibrary = Substitute.For<TagsOwner<Tag>>();
		var set = new OverrideLibrariesClassifierDataSet(innerSet)
		{
			TagsLibrary = tagsLibrary
		};
		set.TagsLibrary.Should().Be(tagsLibrary);
	}

	[Fact]
	public void ShouldOverrideAssetsLibrary()
	{
		var innerSet = Substitute.For<ClassifierDataSet>();
		var assetsLibrary = Substitute.For<AssetsOwner<ClassifierAsset>>();
		var set = new OverrideLibrariesClassifierDataSet(innerSet)
		{
			AssetsLibrary = assetsLibrary
		};
		set.AssetsLibrary.Should().Be(assetsLibrary);
	}

	[Fact]
	public void ShouldOverrideWeightsLibrary()
	{
		var innerSet = Substitute.For<ClassifierDataSet>();
		var weightsLibrary = Substitute.For<WeightsLibrary>();
		var set = new OverrideLibrariesClassifierDataSet(innerSet)
		{
			WeightsLibrary = weightsLibrary
		};
		set.WeightsLibrary.Should().Be(weightsLibrary);
	}
}