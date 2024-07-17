using System.Collections.Immutable;
using MemoryPack;
using SightKeeper.Domain.Model.Profiles.Behaviours;

namespace SightKeeper.Data.Binary.Profiles.Modules.Behaviours;

[MemoryPackable]
internal sealed partial class SerializableAimAssistBehaviour : SerializableBehaviour
{
	public ImmutableArray<SerializableAimAssistBehaviourTagOptions> Tags { get; set; }

	public float DirectionCorrectionFactor { get; set; }
	public float GainFactor { get; set; }
	public float AttenuationFactor { get; set; }

	[MemoryPackConstructor]
	public SerializableAimAssistBehaviour(ImmutableArray<SerializableAimAssistBehaviourTagOptions> tags, float directionCorrectionFactor, float gainFactor, float attenuationFactor)
	{
		Tags = tags;
		DirectionCorrectionFactor = directionCorrectionFactor;
		GainFactor = gainFactor;
		AttenuationFactor = attenuationFactor;
	}

	public SerializableAimAssistBehaviour(
		AimAssistBehaviour behaviour,
		ImmutableArray<SerializableAimAssistBehaviourTagOptions> tags)
	{
		Tags = tags;
		DirectionCorrectionFactor = behaviour.DirectionCorrectionFactor;
		GainFactor = behaviour.GainFactor;
		AttenuationFactor = behaviour.AttenuationFactor;
	}
}