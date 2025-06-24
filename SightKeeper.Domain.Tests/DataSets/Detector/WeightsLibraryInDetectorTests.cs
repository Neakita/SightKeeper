using System.Collections.ObjectModel;
using NSubstitute;
using SightKeeper.Domain.DataSets.Detector;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.DataSets.Weights;

namespace SightKeeper.Domain.Tests.DataSets.Detector;

public sealed class WeightsLibraryInDetectorTests
{
	[Fact]
	public void ShouldCreateWeightsWithSingleTag()
	{
		var innerDataSet = Substitute.For<DetectorDataSet>();
		var domainDataSet = new DomainDetectorDataSet(innerDataSet);
		var tag = Substitute.For<Tag>();
		tag.Owner.Returns(domainDataSet.TagsLibrary);
		IReadOnlyCollection<Tag> tags = [tag];
		var weightsMetadata = new WeightsMetadata();
		domainDataSet.WeightsLibrary.CreateWeights(weightsMetadata, tags);
		innerDataSet.WeightsLibrary.Received().CreateWeights(weightsMetadata, tags);
	}

	[Fact]
	public void ShouldCreateWeightsWithMultipleTags()
	{
		var innerDataSet = Substitute.For<DetectorDataSet>();
		var domainDataSet = new DomainDetectorDataSet(innerDataSet);
		var tag1 = Substitute.For<Tag>();
		tag1.Owner.Returns(domainDataSet.TagsLibrary);
		var tag2 = Substitute.For<Tag>();
		tag2.Owner.Returns(domainDataSet.TagsLibrary);
		IReadOnlyCollection<Tag> tags = [tag1, tag2];
		var weightsMetadata = new WeightsMetadata();
		domainDataSet.WeightsLibrary.CreateWeights(weightsMetadata, tags);
		innerDataSet.WeightsLibrary.Received().CreateWeights(weightsMetadata, tags);
	}

	[Fact]
	public void ShouldNotCreateWeightsWithoutTags()
	{
		var innerDataSet = Substitute.For<DetectorDataSet>();
		var domainDataSet = new DomainDetectorDataSet(innerDataSet);
		Assert.Throws<ArgumentException>(() => domainDataSet.WeightsLibrary.CreateWeights(new WeightsMetadata(), ReadOnlyCollection<Tag>.Empty));
		innerDataSet.WeightsLibrary.DidNotReceive().CreateWeights(Arg.Any<WeightsMetadata>(), Arg.Any<ReadOnlyCollection<Tag>>());
	}
}