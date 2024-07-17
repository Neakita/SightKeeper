using FlakeId;
using MemoryPack;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.Profiles.Behaviours;

namespace SightKeeper.Data.Binary.Profiles.Modules.Behaviours;

[MemoryPackable]
internal sealed partial class SerializableAimAssistBehaviourTagOptions
{
	public Id TagId { get; set; }
	public byte Priority { get; set; }
	public Vector2<float> TargetAreaScale { get; set; }
	public float VerticalOffset { get; set; }

	[MemoryPackConstructor]
	public SerializableAimAssistBehaviourTagOptions(Id tagId, byte priority, Vector2<float> targetAreaScale, float verticalOffset)
	{
		TagId = tagId;
		Priority = priority;
		TargetAreaScale = targetAreaScale;
		VerticalOffset = verticalOffset;
	}

	public SerializableAimAssistBehaviourTagOptions(AimAssistBehaviour.TagOptions options)
	{
		Priority = options.Priority;
		TargetAreaScale = options.TargetAreaScale;
		VerticalOffset = options.VerticalOffset;
	}
}