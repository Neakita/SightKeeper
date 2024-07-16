using System.Collections.Immutable;
using SightKeeper.Domain.Model.DataSets;
using SightKeeper.Domain.Model.DataSets.Classifier;
using SightKeeper.Domain.Model.Profiles;
using SightKeeper.Domain.Tests.DataSets;
using SightKeeper.Domain.Tests.Profiles.Behaviours;

namespace SightKeeper.Domain.Tests.Profiles.Modules;

public class ClassifierModuleTests
{
	[Fact]
	public void ShouldNotInvalidateTagsWhenSettingWeights()
	{
		SimpleWeightsDataAccess weightsDataAccess = new();
		ClassifierDataSet dataSet1 = new("", 320);
		var tag1 = dataSet1.Tags.CreateTag("1");
		var tag2 = dataSet1.Tags.CreateTag("2");
		var weights1 = weightsDataAccess.CreateWeights(dataSet1, [], ModelSize.Nano, new WeightsMetrics(), [tag1, tag2]);
		ClassifierDataSet dataSet2 = new("", 320);
		var tag3 = dataSet2.Tags.CreateTag("3");
		var tag4 = dataSet2.Tags.CreateTag("4");
		var weights2 = weightsDataAccess.CreateWeights(dataSet2, [], ModelSize.Nano, new WeightsMetrics(), [tag3, tag4]);
		Profile profile = new("");
		var module = profile.CreateModule(weights1);
		var tagsBuilder = ImmutableDictionary.CreateBuilder<Tag, SightKeeper.Domain.Model.Profiles.Actions.Action>();
		tagsBuilder.Add(tag1, new FakeAction());
		module.Behaviour.Actions = tagsBuilder.ToImmutable();
		Assert.ThrowsAny<Exception>(() => module.SetWeights(weights2, tagsBuilder.ToImmutable()));
	}
}