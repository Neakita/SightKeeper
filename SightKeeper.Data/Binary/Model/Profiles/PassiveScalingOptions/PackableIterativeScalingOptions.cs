using MemoryPack;
using SightKeeper.Domain.Model.Profiles.Modules.Scaling;

namespace SightKeeper.Data.Binary.Model.Profiles.PassiveScalingOptions;

/// <summary>
/// MemoryPackable version of <see cref="IterativeScalingOptions"/>
/// </summary>
[MemoryPackable]
internal sealed partial class PackableIterativeScalingOptions : PackablePassiveScalingOptions
{
	public float Initial { get; }
	public float StepSize { get; }
	public byte StepsCount { get; }

	public PackableIterativeScalingOptions(float initial, float stepSize, byte stepsCount)
	{
		Initial = initial;
		StepSize = stepSize;
		StepsCount = stepsCount;
	}
}