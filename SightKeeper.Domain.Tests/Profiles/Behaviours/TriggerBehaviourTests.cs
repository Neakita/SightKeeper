using System.Collections.Immutable;
using FluentAssertions;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.DataSets.Classifier;
using SightKeeper.Domain.Model.DataSets.Tags;
using SightKeeper.Domain.Model.DataSets.Weights;
using SightKeeper.Domain.Model.Profiles;
using Action = SightKeeper.Domain.Model.Profiles.Actions.Action;

namespace SightKeeper.Domain.Tests.Profiles.Behaviours;

public sealed class TriggerBehaviourTests
{
	[Fact]
	public void ShouldSetTags()
	{
		ClassifierDataSet dataSet = new();
		var tag1 = dataSet.TagsLibrary.CreateTag("1");
		var tag2 = dataSet.TagsLibrary.CreateTag("2");
		var weights = dataSet.WeightsLibrary.CreateWeights(DateTime.UtcNow, ModelSize.Nano, new WeightsMetrics(), new Vector2<ushort>(320, 320), [tag1, tag2]);
		Profile profile = new("");
		var module = profile.CreateModule(weights);
		var tagsBuilder = ImmutableDictionary.CreateBuilder<Tag, Action>();
		tagsBuilder.Add(tag2, new FakeAction());
		module.Behaviour.Actions = tagsBuilder.ToImmutable();
		module.Behaviour.Actions.Should().ContainKey(tag2);
	}

	[Fact]
	public void ShouldNotSetTagsWithWrongOwnership()
	{
		ClassifierDataSet dataSet1 = new();
		var tag1 = dataSet1.TagsLibrary.CreateTag("1");
		var tag2 = dataSet1.TagsLibrary.CreateTag("2");
		var weights1 = dataSet1.WeightsLibrary.CreateWeights(DateTime.UtcNow, ModelSize.Nano, new WeightsMetrics(), new Vector2<ushort>(320, 320), [tag1, tag2]);
		ClassifierDataSet dataSet2 = new();
		var tag3 = dataSet2.TagsLibrary.CreateTag("3");
		var tag4 = dataSet2.TagsLibrary.CreateTag("4");
		dataSet2.WeightsLibrary.CreateWeights(DateTime.UtcNow, ModelSize.Nano, new WeightsMetrics(), new Vector2<ushort>(320, 320), [tag3, tag4]);
		Profile profile = new("");
		var module = profile.CreateModule(weights1);
		var tagsBuilder = ImmutableDictionary.CreateBuilder<Tag, Action>();
		tagsBuilder.Add(tag3, new FakeAction());
		Assert.ThrowsAny<Exception>(() => module.Behaviour.Actions = tagsBuilder.ToImmutable());
	}
}