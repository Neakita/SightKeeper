﻿using System.Collections.Immutable;
using FluentAssertions;
using SightKeeper.Domain.Model.DataSets;
using SightKeeper.Domain.Model.DataSets.Classifier;
using SightKeeper.Domain.Model.Profiles;
using SightKeeper.Domain.Tests.DataSets;
using Action = SightKeeper.Domain.Model.Profiles.Actions.Action;

namespace SightKeeper.Domain.Tests.Profiles.Behaviours;

public sealed class TriggerBehaviourTests
{
	[Fact]
	public void ShouldSetTags()
	{
		SimpleWeightsDataAccess weightsDataAccess = new();
		ClassifierDataSet dataSet = new();
		var tag1 = dataSet.Tags.CreateTag("1");
		var tag2 = dataSet.Tags.CreateTag("2");
		var weights = weightsDataAccess.CreateWeights(dataSet.Weights, [], ModelSize.Nano, new WeightsMetrics(), [tag1, tag2]);
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
		SimpleWeightsDataAccess weightsDataAccess = new();
		ClassifierDataSet dataSet1 = new();
		var tag1 = dataSet1.Tags.CreateTag("1");
		var tag2 = dataSet1.Tags.CreateTag("2");
		var weights1 = weightsDataAccess.CreateWeights(dataSet1.Weights, [], ModelSize.Nano, new WeightsMetrics(), [tag1, tag2]);
		ClassifierDataSet dataSet2 = new();
		var tag3 = dataSet2.Tags.CreateTag("3");
		var tag4 = dataSet2.Tags.CreateTag("4");
		weightsDataAccess.CreateWeights(dataSet2.Weights, [], ModelSize.Nano, new WeightsMetrics(), [tag3, tag4]);
		Profile profile = new("");
		var module = profile.CreateModule(weights1);
		var tagsBuilder = ImmutableDictionary.CreateBuilder<Tag, Action>();
		tagsBuilder.Add(tag3, new FakeAction());
		Assert.ThrowsAny<Exception>(() => module.Behaviour.Actions = tagsBuilder.ToImmutable());
	}
}