using FluentAssertions;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.DataSets.Detector;
using SightKeeper.Domain.DataSets.Poser;
using SightKeeper.Domain.DataSets.Poser2D;
using SightKeeper.Domain.DataSets.Weights;

namespace SightKeeper.Domain.Tests.DataSets;

public sealed class WeightsLibraryTests
{
	[Fact]
	public void ShouldCreateWeights()
	{
		var dataSet = CreateDataSet();
		var tag = dataSet.TagsLibrary.CreateTag("");
		var weights = Utilities.CreateWeights(tag);
		dataSet.WeightsLibrary.AddWeights(weights);
		dataSet.WeightsLibrary.Weights.Should().Contain(weights);
	}

	[Fact]
	public void ShouldNotCreateWeightsWithNoTags()
	{
		var dataSet = CreateDataSet();
		var weights = Utilities.CreateWeights();
		Assert.Throws<ArgumentException>(() => dataSet.WeightsLibrary.AddWeights(weights));
		dataSet.WeightsLibrary.Weights.Should().BeEmpty();
	}

	[Fact]
	public void ShouldNotCreateWeightsWithDuplicateTags()
	{
		var dataSet = CreateDataSet();
		var tag1 = dataSet.TagsLibrary.CreateTag("1");
		var tag2 = dataSet.TagsLibrary.CreateTag("2");
		var weights = Utilities.CreateWeights(tag1, tag1, tag2);
		Assert.Throws<DuplicateTagsException>(() => dataSet.WeightsLibrary.AddWeights(weights));
		dataSet.WeightsLibrary.Weights.Should().BeEmpty();
	}

	[Fact]
	public void ShouldCreateWeightsWithPoserTag()
	{
		var dataSet = CreatePoserDataSet();
		var tag = dataSet.TagsLibrary.CreateTag("");
		var weights = Utilities.CreateWeights(tag);
		dataSet.WeightsLibrary.AddWeights(weights);
		dataSet.WeightsLibrary.Weights.Should().Contain(weights);
	}

	[Fact]
	public void ShouldNotCreateWeightsWithKeyPointTagsWithoutItsOwner()
	{
		var dataSet = CreatePoserDataSet();
		var tag1 = dataSet.TagsLibrary.CreateTag("1");
		tag1.CreateKeyPointTag("1.1");
		var tag2 = dataSet.TagsLibrary.CreateTag("2");
		var keyPoint2 = tag2.CreateKeyPointTag("2.1");
		var weights = Utilities.CreateWeights(tag1, keyPoint2);
		var exception = Assert.Throws<KeyPointTagWithoutOwnerException>(() => dataSet.WeightsLibrary.AddWeights(weights));
		dataSet.WeightsLibrary.Weights.Should().BeEmpty();
		exception.ExpectedOwner.Should().Be(tag2);
		exception.KeyPointTag.Should().Be(keyPoint2);
	}

	[Fact]
	public void ShouldCreateWeightsWithKeyPointTag()
	{
		var dataSet = CreatePoserDataSet();
		var tag = dataSet.TagsLibrary.CreateTag("1");
		var keyPointTag = tag.CreateKeyPointTag("1.1");
		var weights = Utilities.CreateWeights(tag, keyPointTag);
		dataSet.WeightsLibrary.AddWeights(weights);
		dataSet.WeightsLibrary.Weights.Should().Contain(weights).Which.Tags.Should().Contain([tag, keyPointTag]);
	}

	private static DataSet CreateDataSet()
	{
		return new DetectorDataSet();
	}

	private static PoserDataSet CreatePoserDataSet()
	{
		return new Poser2DDataSet();
	}
}