using FluentAssertions;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.DataSets.Classifier;
using SightKeeper.Domain.Model.DataSets.Weights;
using SightKeeper.Domain.Model.Profiles;
using SightKeeper.Domain.Model.Profiles.Behaviors;

namespace SightKeeper.Domain.Tests.Profiles.Behaviors;

public sealed class TriggerBehaviorTests
{
	[Fact]
	public void ShouldSetTags()
	{
		ClassifierDataSet dataSet = new();
		var tag1 = dataSet.TagsLibrary.CreateTag("1");
		var tag2 = dataSet.TagsLibrary.CreateTag("2");
		var weights = dataSet.WeightsLibrary.CreateWeights(DateTime.UtcNow, ModelSize.Nano, new WeightsMetrics(), new Vector2<ushort>(320, 320), [tag1, tag2], null);
		Profile profile = new("");
		var module = profile.CreateModule(weights);
		module.Behavior.Tags = [new TriggerBehavior.TagOptions(tag2, new FakeAction())];
		module.Behavior.Tags.Single().Tag.Should().Be(tag2);
	}

	[Fact]
	public void ShouldNotSetTagsWithWrongOwnership()
	{
		ClassifierDataSet dataSet1 = new();
		var tag1 = dataSet1.TagsLibrary.CreateTag("1");
		var tag2 = dataSet1.TagsLibrary.CreateTag("2");
		var weights1 = dataSet1.WeightsLibrary.CreateWeights(DateTime.UtcNow, ModelSize.Nano, new WeightsMetrics(), new Vector2<ushort>(320, 320), [tag1, tag2], null);
		ClassifierDataSet dataSet2 = new();
		var tag3 = dataSet2.TagsLibrary.CreateTag("3");
		var tag4 = dataSet2.TagsLibrary.CreateTag("4");
		dataSet2.WeightsLibrary.CreateWeights(DateTime.UtcNow, ModelSize.Nano, new WeightsMetrics(), new Vector2<ushort>(320, 320), [tag3, tag4], null);
		Profile profile = new("");
		var module = profile.CreateModule(weights1);
		Assert.ThrowsAny<Exception>(() => module.Behavior.Tags = [new TriggerBehavior.TagOptions(tag3, new FakeAction())]);
	}
}