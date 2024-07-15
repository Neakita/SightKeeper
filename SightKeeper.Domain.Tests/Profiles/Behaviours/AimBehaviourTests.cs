using System.Collections.Immutable;
using FluentAssertions;
using SightKeeper.Domain.Model.DataSets;
using SightKeeper.Domain.Model.DataSets.Detector;
using SightKeeper.Domain.Model.Profiles;
using SightKeeper.Domain.Tests.DataSets;

namespace SightKeeper.Domain.Tests.Profiles.Behaviours;

public sealed class AimBehaviourTests
{
	[Fact]
	public void ShouldSetTags()
	{
		SimpleDetectorWeightsDataAccess weightsDataAccess = new();
		DetectorDataSet dataSet = new("", 320);
		var tag = dataSet.Tags.CreateTag("");
		var weights = weightsDataAccess.CreateWeights(dataSet, [], ModelSize.Nano, new WeightsMetrics(), [tag]);
		Profile profile = new("");
		var module = profile.CreateModule(weights);
		var tagsBuilder = ImmutableDictionary.CreateBuilder<Tag, Model.Profiles.Behaviours.AimBehaviour.TagOptions>();
		tagsBuilder.Add(tag, new Model.Profiles.Behaviours.AimBehaviour.TagOptions());
		module.Behaviour.Tags = tagsBuilder.ToImmutable();
		module.Behaviour.Tags.Should().ContainKey(tag);
	}

	[Fact]
	public void ShouldNotSetTagsWithWrongOwnership()
	{
		SimpleDetectorWeightsDataAccess weightsDataAccess = new();
		DetectorDataSet dataSet1 = new("", 320);
		var tag1 = dataSet1.Tags.CreateTag("");
		var weights1 = weightsDataAccess.CreateWeights(dataSet1, [], ModelSize.Nano, new WeightsMetrics(), [tag1]);
		DetectorDataSet dataSet2 = new("", 320);
		var tag2 = dataSet2.Tags.CreateTag("");
		weightsDataAccess.CreateWeights(dataSet2, [], ModelSize.Nano, new WeightsMetrics(), [tag2]);
		Profile profile = new("");
		var module = profile.CreateModule(weights1);
		var tagsBuilder = ImmutableDictionary.CreateBuilder<Tag, Model.Profiles.Behaviours.AimBehaviour.TagOptions>();
		tagsBuilder.Add(tag2, new Model.Profiles.Behaviours.AimBehaviour.TagOptions());
		Assert.ThrowsAny<Exception>(() => module.Behaviour.Tags = tagsBuilder.ToImmutable());
	}
}