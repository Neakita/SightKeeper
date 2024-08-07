using MemoryPack;
using SightKeeper.Domain.Model;

namespace SightKeeper.Data.Binary.Profiles.Modules.Walking;

[MemoryPackable]
internal sealed partial class IterativeWalkingOptions : PassiveWalkingOptions
{
	public Vector2<float> OffsetStep { get; }
	public Vector2<byte> MaximumSteps { get; }

	[MemoryPackConstructor]
	public IterativeWalkingOptions(Vector2<float> offsetStep, Vector2<byte> maximumSteps)
	{
		OffsetStep = offsetStep;
		MaximumSteps = maximumSteps;
	}

	public IterativeWalkingOptions(Domain.Model.Profiles.Modules.Walking.IterativeWalkingOptions options)
	{
		OffsetStep = options.OffsetStep;
		MaximumSteps = options.MaximumSteps;
	}
}