using MemoryPack;

namespace SightKeeper.Data.Binary.Profiles.Modules.Scaling;

[MemoryPackable]
internal sealed partial class IterativeScalingOptions : PassiveScalingOptions
{
	public float Initial { get; set; }
	public float StepSize { get; set; }
	public byte StepsCount { get; set; }

	[MemoryPackConstructor]
	public IterativeScalingOptions(float initial, float stepSize, byte stepsCount)
	{
		Initial = initial;
		StepSize = stepSize;
		StepsCount = stepsCount;
	}

	public IterativeScalingOptions(Domain.Model.Profiles.Modules.Scaling.IterativeScalingOptions options)
	{
		Initial = options.Initial;
		StepSize = options.StepSize;
		StepsCount = options.StepsCount;
	}

	public override Domain.Model.Profiles.Modules.Scaling.IterativeScalingOptions Convert() => new(Initial, StepSize, StepsCount);
}