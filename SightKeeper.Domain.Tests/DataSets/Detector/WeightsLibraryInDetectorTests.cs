using NSubstitute;
using SightKeeper.Domain.DataSets.Detector;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.DataSets.Weights;

namespace SightKeeper.Domain.Tests.DataSets.Detector;

public sealed class WeightsLibraryInDetectorTests
{
	[Fact]
	public void ShouldAddWeightsWithSingleTag()
	{
		var innerDataSet = Substitute.For<DetectorDataSet>();
		var domainDataSet = new DomainDetectorDataSet(innerDataSet);
		var weights = Substitute.For<Weights>();
		var tag = Substitute.For<Tag>();
		tag.Owner.Returns(domainDataSet.TagsLibrary);
		weights.Tags.Returns([tag]);
		domainDataSet.WeightsLibrary.AddWeights(weights);
		innerDataSet.WeightsLibrary.Received().AddWeights(weights);
	}

	[Fact]
	public void ShouldAddWeightsWithMultipleTags()
	{
		var innerDataSet = Substitute.For<DetectorDataSet>();
		var domainDataSet = new DomainDetectorDataSet(innerDataSet);
		var weights = Substitute.For<Weights>();
		var tag1 = Substitute.For<Tag>();
		tag1.Owner.Returns(domainDataSet.TagsLibrary);
		var tag2 = Substitute.For<Tag>();
		tag2.Owner.Returns(domainDataSet.TagsLibrary);
		weights.Tags.Returns([tag1, tag2]);
		domainDataSet.WeightsLibrary.AddWeights(weights);
		innerDataSet.WeightsLibrary.Received().AddWeights(weights);
	}

	[Fact]
	public void ShouldNotAddWeightsWithoutTags()
	{
		var innerDataSet = Substitute.For<DetectorDataSet>();
		var domainDataSet = new DomainDetectorDataSet(innerDataSet);
		var weights = Substitute.For<Weights>();
		Assert.Throws<ArgumentException>(() => domainDataSet.WeightsLibrary.AddWeights(weights));
		innerDataSet.WeightsLibrary.DidNotReceive().AddWeights(Arg.Any<Weights>());
	}
}