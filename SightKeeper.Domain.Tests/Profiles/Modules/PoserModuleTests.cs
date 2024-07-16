using System.Collections.Immutable;
using FluentAssertions;
using SightKeeper.Domain.Model.DataSets;
using SightKeeper.Domain.Model.DataSets.Poser;
using SightKeeper.Domain.Model.Profiles;
using SightKeeper.Domain.Tests.DataSets;

namespace SightKeeper.Domain.Tests.Profiles.Modules;

public class PoserModuleTests
{
	[Fact]
	public void ShouldNotInvalidateTagsWhenSettingWeights()
	{
		SimpleWeightsDataAccess weightsDataAccess = new();
		PoserDataSet dataSet1 = new("", 320);
		var tag1 = dataSet1.Tags.CreateTag("");
		var weightsTagsBuilder1 = ImmutableDictionary.CreateBuilder<PoserTag, ImmutableHashSet<KeyPointTag>>();
		weightsTagsBuilder1.Add(tag1, []);
		var weights1 = weightsDataAccess.CreateWeights(dataSet1, [], ModelSize.Nano, new WeightsMetrics(), weightsTagsBuilder1.ToImmutable());
		PoserDataSet dataSet2 = new("", 320);
		var tag2 = dataSet2.Tags.CreateTag("");
		var weightsTagsBuilder2 = ImmutableDictionary.CreateBuilder<PoserTag, ImmutableHashSet<KeyPointTag>>();
		weightsTagsBuilder2.Add(tag2, []);
		var weights2 = weightsDataAccess.CreateWeights(dataSet2, [], ModelSize.Nano, new WeightsMetrics(), weightsTagsBuilder2.ToImmutable());
		Profile profile = new("");
		var module = profile.CreateModule(weights1);
		var tagsBuilder = ImmutableDictionary.CreateBuilder<Tag, Model.Profiles.Behaviours.AimBehaviour.TagOptions>();
		tagsBuilder.Add(tag1, new Model.Profiles.Behaviours.AimBehaviour.TagOptions());
		module.Behaviour.Tags = tagsBuilder.ToImmutable();
		Assert.ThrowsAny<Exception>(() => module.SetWeights(weights2, tagsBuilder.ToImmutable()));
		module.Behaviour.Tags.Should().NotContainKey(tag1);
	}
}