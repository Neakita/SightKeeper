using System.Collections.Immutable;
using FluentAssertions;
using SightKeeper.Domain.Model.DataSets;
using SightKeeper.Domain.Model.DataSets.Detector;
using SightKeeper.Domain.Model.Profiles;
using SightKeeper.Domain.Tests.DataSets;

namespace SightKeeper.Domain.Tests.Profiles.Modules;

public sealed class DetectorModuleTests
{
	[Fact]
	public void ShouldNotInvalidateTagsWhenSettingWeights()
	{
		SimpleWeightsDataAccess weightsDataAccess = new();
		DetectorDataSet dataSet1 = new("", 320);
		var tag1 = dataSet1.Tags.CreateTag("");
		var weights1 = weightsDataAccess.CreateWeights(dataSet1, [], ModelSize.Nano, new WeightsMetrics(), [tag1]);
		DetectorDataSet dataSet2 = new("", 320);
		var tag2 = dataSet2.Tags.CreateTag("");
		var weights2 = weightsDataAccess.CreateWeights(dataSet2, [], ModelSize.Nano, new WeightsMetrics(), [tag2]);
		Profile profile = new("");
		var module = profile.CreateModule(weights1);
		var tagsBuilder = ImmutableDictionary.CreateBuilder<Tag, Model.Profiles.Behaviours.AimBehaviour.TagOptions>();
		tagsBuilder.Add(tag1, new Model.Profiles.Behaviours.AimBehaviour.TagOptions());
		module.Behaviour.Tags = tagsBuilder.ToImmutable();
		Assert.ThrowsAny<Exception>(() => module.SetWeights(weights2, tagsBuilder.ToImmutable()));
		module.Behaviour.Tags.Should().NotContainKey(tag1);
	}
}