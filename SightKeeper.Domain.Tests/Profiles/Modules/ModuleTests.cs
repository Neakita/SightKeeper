using FluentAssertions;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.DataSets.Detector;
using SightKeeper.Domain.Model.DataSets.Weights;
using SightKeeper.Domain.Model.Profiles;
using SightKeeper.Domain.Model.Profiles.Modules.Scaling;
using SightKeeper.Domain.Model.Profiles.Modules.Walking;

namespace SightKeeper.Domain.Tests.Profiles.Modules;

public sealed class ModuleTests
{
	[Fact]
	public void ShouldNotSetIterativeScalingAndIterativeWalkingOptionsTogether()
	{
		DetectorDataSet dataSet = new();
		var tag = dataSet.TagsLibrary.CreateTag("");
		var weights = dataSet.WeightsLibrary.CreateWeights(DateTime.UtcNow, ModelSize.Nano, new WeightsMetrics(), new Vector2<ushort>(320, 320), [tag]);
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