using FluentAssertions;
using SightKeeper.Domain.DataSets.Classifier;

namespace SightKeeper.Domain.Tests.DataSets.Classifier;

public sealed class WeightsLibraryInClassifierTests
{
	[Fact]
	public void ShouldCreateWeightsWithTwoTags()
	{
		ClassifierDataSet dataSet = new();
		var tag1 = dataSet.TagsLibrary.CreateTag("1");
		var tag2 = dataSet.TagsLibrary.CreateTag("2");
		var weights = dataSet.WeightsLibrary.CreateWeights(tag1, tag2);
		dataSet.WeightsLibrary.Weights.Should().Contain(weights);
	}

	[Fact]
	public void ShouldNotCreateWeightsWithOneTag()
	{
		ClassifierDataSet dataSet = new();
		var tag = dataSet.TagsLibrary.CreateTag("");
		Assert.Throws<ArgumentException>(() => dataSet.WeightsLibrary.CreateWeights(tag));
		dataSet.WeightsLibrary.Weights.Should().BeEmpty();
	}
}