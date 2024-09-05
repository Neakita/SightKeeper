using System.Collections.Immutable;
using FluentAssertions;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.DataSets.Detector;
using SightKeeper.Domain.Model.DataSets.Tags;
using SightKeeper.Domain.Model.DataSets.Weights;
using SightKeeper.Domain.Model.Profiles;
using SightKeeper.Domain.Model.Profiles.Behaviours;

namespace SightKeeper.Domain.Tests.Profiles.Behaviours;

public sealed class AimBehaviourTests
{
	[Fact]
	public void ShouldSetTags()
	{
		DetectorDataSet dataSet = new();
		var tag = dataSet.Tags.CreateTag("");
		var weights = dataSet.Weights.CreateWeights(DateTime.UtcNow, ModelSize.Nano, new WeightsMetrics(), new Vector2<ushort>(320, 320), [tag]);
		Profile profile = new("");
		var module = profile.CreateModule(weights);
		var tagsBuilder = ImmutableDictionary.CreateBuilder<Tag, AimBehaviour.TagOptions>();
		tagsBuilder.Add(tag, new AimBehaviour.TagOptions(0, 0));
		var behaviour = module.SetAimBehaviour();
		behaviour.Tags = tagsBuilder.ToImmutable();
		behaviour.Tags.Should().ContainKey(tag);
	}

	[Fact]
	public void ShouldNotSetTagsWithWrongOwnership()
	{
		DetectorDataSet dataSet1 = new();
		var tag1 = dataSet1.Tags.CreateTag("");
		var weights1 = dataSet1.Weights.CreateWeights(DateTime.UtcNow, ModelSize.Nano, new WeightsMetrics(), new Vector2<ushort>(320, 320), [tag1]);
		DetectorDataSet dataSet2 = new();
		var tag2 = dataSet2.Tags.CreateTag("");
		dataSet2.Weights.CreateWeights(DateTime.UtcNow, ModelSize.Nano, new WeightsMetrics(), new Vector2<ushort>(320, 320), [tag2]);
		Profile profile = new("");
		var module = profile.CreateModule(weights1);
		var tagsBuilder = ImmutableDictionary.CreateBuilder<Tag, AimBehaviour.TagOptions>();
		tagsBuilder.Add(tag2, new AimBehaviour.TagOptions(0, 0));
		var behaviour = module.SetAimBehaviour();
		Assert.ThrowsAny<Exception>(() => behaviour.Tags = tagsBuilder.ToImmutable());
	}
}