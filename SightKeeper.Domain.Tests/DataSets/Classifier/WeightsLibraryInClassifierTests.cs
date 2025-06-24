using NSubstitute;
using SightKeeper.Domain.DataSets.Classifier;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.DataSets.Weights;

namespace SightKeeper.Domain.Tests.DataSets.Classifier;

public sealed class WeightsLibraryInClassifierTests
{
	[Fact]
	public void ShouldCreateWeightsWithTwoTags()
	{
		var innerSet = Substitute.For<ClassifierDataSet>();
		DomainClassifierDataSet domainSet = new(innerSet);
		var tag1 = Substitute.For<Tag>();
		tag1.Owner.Returns(domainSet.TagsLibrary);
		var tag2 = Substitute.For<Tag>();
		tag2.Owner.Returns(domainSet.TagsLibrary);
		IReadOnlyCollection<Tag> tags = [tag1, tag2];
		var weightsMetadata = new WeightsMetadata();
		domainSet.WeightsLibrary.CreateWeights(weightsMetadata, tags);
		innerSet.WeightsLibrary.Received().CreateWeights(weightsMetadata, tags);
	}

	[Fact]
	public void ShouldNotAddWeightsWithOneTag()
	{
		var innerSet = Substitute.For<ClassifierDataSet>();
		DomainClassifierDataSet domainSet = new(innerSet);
		var tag = Substitute.For<Tag>();
		tag.Owner.Returns(domainSet.TagsLibrary);
		var weights = Substitute.For<Weights>();
		weights.Tags.Returns([tag]);
		var weightsMetadata = new WeightsMetadata();
		Assert.Throws<ArgumentException>(() => domainSet.WeightsLibrary.CreateWeights(weightsMetadata, [tag]));
		innerSet.WeightsLibrary.DidNotReceive().CreateWeights(Arg.Any<WeightsMetadata>(), Arg.Any<IReadOnlyCollection<Tag>>());
	}
}