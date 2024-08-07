using FluentAssertions;
using SightKeeper.Domain.Model.DataSets;
using SightKeeper.Domain.Model.DataSets.Detector;
using SightKeeper.Domain.Model.Profiles;
using SightKeeper.Domain.Model.Profiles.Modules.Scaling;
using SightKeeper.Domain.Model.Profiles.Modules.Walking;
using SightKeeper.Domain.Tests.DataSets;

namespace SightKeeper.Domain.Tests.Profiles.Modules;

public sealed class ModuleTests
{
	[Fact]
	public void ShouldNotSetIterativeScalingAndIterativeWalkingOptionsTogether()
	{
		SimpleWeightsDataAccess weightsDataAccess = new();
		DetectorDataSet dataSet = new();
		var tag = dataSet.Tags.CreateTag("");
		var weights = weightsDataAccess.CreateWeights(dataSet.Weights, [], ModelSize.Nano, new WeightsMetrics(), [tag]);
		Profile profile = new("");
		var module = profile.CreateModule(weights);
		module.PassiveScalingOptions = new IterativeScalingOptions();
		Assert.ThrowsAny<Exception>(() => module.PassiveWalkingOptions = new IterativeWalkingOptions());
		module.PassiveWalkingOptions.Should().BeNull();
		module.PassiveScalingOptions = null;
		module.PassiveWalkingOptions = new IterativeWalkingOptions();
		Assert.ThrowsAny<Exception>(() => module.PassiveScalingOptions = new IterativeScalingOptions());
		module.PassiveScalingOptions.Should().BeNull();
	}
}