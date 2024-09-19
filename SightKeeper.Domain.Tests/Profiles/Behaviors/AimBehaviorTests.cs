using System.Collections.Immutable;
using FluentAssertions;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.DataSets.Detector;
using SightKeeper.Domain.Model.DataSets.Weights;
using SightKeeper.Domain.Model.Profiles;
using SightKeeper.Domain.Model.Profiles.Behaviors;

namespace SightKeeper.Domain.Tests.Profiles.Behaviors;

public sealed class AimBehaviorTests
{
	[Fact]
	public void ShouldSetTags()
	{
		DetectorDataSet dataSet = new();
		var tag = dataSet.TagsLibrary.CreateTag("");
		var weights = dataSet.WeightsLibrary.CreateWeights(DateTime.UtcNow, ModelSize.Nano, new WeightsMetrics(), new Vector2<ushort>(320, 320), [tag], null);
		Profile profile = new("");
		var module = profile.CreateModule(weights);
		var tagsBuilder = ImmutableArray.CreateBuilder<AimBehavior.TagOptions>();
		tagsBuilder.Add(new AimBehavior.TagOptions(tag, 0, 0));
		var behavior = module.SetBehavior<AimBehavior>();
		behavior.Tags = tagsBuilder.ToImmutable();
		behavior.Tags.First().Tag.Should().Be(tag);
	}

	[Fact]
	public void ShouldNotSetTagsWithWrongOwnership()
	{
		DetectorDataSet dataSet1 = new();
		var tag1 = dataSet1.TagsLibrary.CreateTag("");
		var weights1 = dataSet1.WeightsLibrary.CreateWeights(DateTime.UtcNow, ModelSize.Nano, new WeightsMetrics(), new Vector2<ushort>(320, 320), [tag1], null);
		DetectorDataSet dataSet2 = new();
		var tag2 = dataSet2.TagsLibrary.CreateTag("");
		dataSet2.WeightsLibrary.CreateWeights(DateTime.UtcNow, ModelSize.Nano, new WeightsMetrics(), new Vector2<ushort>(320, 320), [tag2], null);
		Profile profile = new("");
		var module = profile.CreateModule(weights1);
		var tagsBuilder = ImmutableArray.CreateBuilder<AimBehavior.TagOptions>();
		tagsBuilder.Add(new AimBehavior.TagOptions(tag2, 0, 0));
		var behavior = module.SetBehavior<AimBehavior>();
		Assert.ThrowsAny<Exception>(() => behavior.Tags = tagsBuilder.ToImmutable());
	}
}