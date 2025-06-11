using FluentAssertions;
using SightKeeper.Domain.DataSets.Classifier;

namespace SightKeeper.Domain.Tests.DataSets.Classifier;

public sealed class WeightsLibraryInClassifierTests
{
	[Fact]
	public void ShouldAddWeightsWithTwoTags()
	{
		DomainClassifierDataSet dataSet = new();
		var tag1 = dataSet.TagsLibrary.CreateTag("1");
		var tag2 = dataSet.TagsLibrary.CreateTag("2");
		var weights = Utilities.CreateWeights(tag1, tag2);
		dataSet.WeightsLibrary.AddWeights(weights);
		dataSet.WeightsLibrary.Weights.Should().Contain(weights);
	}

	[Fact]
	public void ShouldNotAddWeightsWithOneTag()
	{
		DomainClassifierDataSet dataSet = new();
		var tag = dataSet.TagsLibrary.CreateTag("");
		var weights = Utilities.CreateWeights(tag);
		Assert.Throws<ArgumentException>(() => dataSet.WeightsLibrary.AddWeights(weights));
		dataSet.WeightsLibrary.Weights.Should().BeEmpty();
	}
}