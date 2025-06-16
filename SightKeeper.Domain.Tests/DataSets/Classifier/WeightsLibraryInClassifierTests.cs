using NSubstitute;
using SightKeeper.Domain.DataSets.Classifier;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.DataSets.Weights;

namespace SightKeeper.Domain.Tests.DataSets.Classifier;

public sealed class WeightsLibraryInClassifierTests
{
	[Fact]
	public void ShouldAddWeightsWithTwoTags()
	{
		var innerSet = Substitute.For<ClassifierDataSet>();
		DomainClassifierDataSet domainSet = new(innerSet);
		var tag1 = Substitute.For<Tag>();
		tag1.Owner.Returns(domainSet.TagsLibrary);
		var tag2 = Substitute.For<Tag>();
		tag2.Owner.Returns(domainSet.TagsLibrary);
		var weights = Substitute.For<Weights>();
		weights.Tags.Returns([tag1, tag2]);
		domainSet.WeightsLibrary.AddWeights(weights);
		innerSet.WeightsLibrary.Received().AddWeights(weights);
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
		Assert.Throws<ArgumentException>(() => domainSet.WeightsLibrary.AddWeights(weights));
		innerSet.WeightsLibrary.DidNotReceive().AddWeights(Arg.Any<Weights>());
	}
}