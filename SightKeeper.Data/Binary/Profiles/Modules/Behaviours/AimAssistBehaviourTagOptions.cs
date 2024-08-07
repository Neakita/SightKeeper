using FlakeId;
using MemoryPack;
using SightKeeper.Domain.Model;

namespace SightKeeper.Data.Binary.Profiles.Modules.Behaviours;

[MemoryPackable]
internal sealed partial class AimAssistBehaviourTagOptions
{
	public Id TagId { get; set; }
	public byte Priority { get; set; }
	public Vector2<float> TargetAreaScale { get; set; }
	public float VerticalOffset { get; set; }

	[MemoryPackConstructor]
	public AimAssistBehaviourTagOptions(Id tagId, byte priority, Vector2<float> targetAreaScale, float verticalOffset)
	{
		TagId = tagId;
		Priority = priority;
		TargetAreaScale = targetAreaScale;
		VerticalOffset = verticalOffset;
	}

	public AimAssistBehaviourTagOptions(Id tagId, Domain.Model.Profiles.Behaviours.AimAssistBehaviour.TagOptions options)
	{
		TagId = tagId;
		Priority = options.Priority;
		TargetAreaScale = options.TargetAreaScale;
		VerticalOffset = options.VerticalOffset;
	}
}