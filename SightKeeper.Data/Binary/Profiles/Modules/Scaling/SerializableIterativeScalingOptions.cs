using MemoryPack;
using SightKeeper.Domain.Model.Profiles.Modules.Scaling;

namespace SightKeeper.Data.Binary.Profiles.Modules.Scaling;

[MemoryPackable]
internal sealed partial class SerializableIterativeScalingOptions : SerializablePassiveScalingOptions
{
	public float Initial { get; set; }
	public float StepSize { get; set; }
	public byte StepsCount { get; set; }

	[MemoryPackConstructor]
	public SerializableIterativeScalingOptions(float initial, float stepSize, byte stepsCount)
	{
		Initial = initial;
		StepSize = stepSize;
		StepsCount = stepsCount;
	}

	public SerializableIterativeScalingOptions(IterativeScalingOptions options)
	{
		Initial = options.Initial;
		StepSize = options.StepSize;
		StepsCount = options.StepsCount;
	}

	public override IterativeScalingOptions Convert() => new(Initial, StepSize, StepsCount);
}