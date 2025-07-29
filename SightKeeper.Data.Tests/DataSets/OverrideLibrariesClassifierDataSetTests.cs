using FluentAssertions;
using NSubstitute;
using SightKeeper.Data.DataSets.Assets;
using SightKeeper.Data.DataSets.Classifier;
using SightKeeper.Data.DataSets.Weights;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Data.Tests.DataSets;

public sealed class OverrideLibrariesClassifierDataSetTests
{
	[Fact]
	public void ShouldOverrideTagsLibrary()
	{
		var innerSet = Substitute.For<StorableClassifierDataSet>();
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
		var innerSet = Substitute.For<StorableClassifierDataSet>();
		var assetsLibrary = Substitute.For<StorableAssetsOwner<StorableClassifierAsset>>();
		var set = new OverrideLibrariesClassifierDataSet(innerSet)
		{
			AssetsLibrary = assetsLibrary
		};
		set.AssetsLibrary.Should().Be(assetsLibrary);
	}

	[Fact]
	public void ShouldOverrideWeightsLibrary()
	{
		var innerSet = Substitute.For<StorableClassifierDataSet>();
		var weightsLibrary = Substitute.For<StorableWeightsLibrary>();
		var set = new OverrideLibrariesClassifierDataSet(innerSet)
		{
			WeightsLibrary = weightsLibrary
		};
		set.WeightsLibrary.Should().Be(weightsLibrary);
	}
}