using MemoryPack;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.Profiles.Modules.Walking;

namespace SightKeeper.Data.Binary.Model.Profiles.PassiveWalkingOptions;

/// <summary>
/// MemoryPackable version of <see cref="IterativeWalkingOptions"/>
/// </summary>
[MemoryPackable]
internal sealed partial class PackableIterativeWalkingOptions : PackablePassiveWalkingOptions
{
	public Vector2<float> OffsetStep { get; }
	public Vector2<byte> StepsCount { get; }

	public PackableIterativeWalkingOptions(Vector2<float> offsetStep, Vector2<byte> stepsCount)
	{
		OffsetStep = offsetStep;
		StepsCount = stepsCount;
	}
}