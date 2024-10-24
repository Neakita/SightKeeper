using System.Collections.Immutable;
using MemoryPack;
using SightKeeper.Domain.Model.Profiles.Behaviors;

namespace SightKeeper.Data.Binary.Model.Profiles.Behaviors;

/// <summary>
/// MemoryPackable version of <see cref="AimAssistBehavior"/>
/// </summary>
[MemoryPackable]
internal sealed partial class PackableAimAssistBehavior : PackableBehavior
{
	public ImmutableArray<PackableAimAssistBehaviorTagOptions> Tags { get; }
	public float DirectionCorrectionFactor { get; }
	public float GainFactor { get; }
	public float AttenuationFactor { get; }

	public PackableAimAssistBehavior(
		ImmutableArray<PackableAimAssistBehaviorTagOptions> tags,
		float directionCorrectionFactor,
		float gainFactor,
		float attenuationFactor)
	{
		Tags = tags;
		DirectionCorrectionFactor = directionCorrectionFactor;
		GainFactor = gainFactor;
		AttenuationFactor = attenuationFactor;
	}
}