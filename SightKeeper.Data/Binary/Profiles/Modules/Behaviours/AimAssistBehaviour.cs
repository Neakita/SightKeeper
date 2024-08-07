using System.Collections.Immutable;
using MemoryPack;

namespace SightKeeper.Data.Binary.Profiles.Modules.Behaviours;

[MemoryPackable]
internal sealed partial class AimAssistBehaviour : Behaviour
{
	public ImmutableArray<AimAssistBehaviourTagOptions> Tags { get; set; }

	public float DirectionCorrectionFactor { get; set; }
	public float GainFactor { get; set; }
	public float AttenuationFactor { get; set; }

	[MemoryPackConstructor]
	public AimAssistBehaviour(ImmutableArray<AimAssistBehaviourTagOptions> tags, float directionCorrectionFactor, float gainFactor, float attenuationFactor)
	{
		Tags = tags;
		DirectionCorrectionFactor = directionCorrectionFactor;
		GainFactor = gainFactor;
		AttenuationFactor = attenuationFactor;
	}

	public AimAssistBehaviour(
		Domain.Model.Profiles.Behaviours.AimAssistBehaviour behaviour,
		ImmutableArray<AimAssistBehaviourTagOptions> tags)
	{
		Tags = tags;
		DirectionCorrectionFactor = behaviour.DirectionCorrectionFactor;
		GainFactor = behaviour.GainFactor;
		AttenuationFactor = behaviour.AttenuationFactor;
	}
}