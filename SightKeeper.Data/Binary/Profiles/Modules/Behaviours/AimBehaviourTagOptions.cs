using FlakeId;
using MemoryPack;

namespace SightKeeper.Data.Binary.Profiles.Modules.Behaviours;

[MemoryPackable]
internal sealed partial class AimBehaviourTagOptions
{
	public Id TagId { get; set; }
	public byte Priority { get; set; }
	public float VerticalOffset { get; set; }

	[MemoryPackConstructor]
	public AimBehaviourTagOptions(Id tagId, byte priority, float verticalOffset)
	{
		TagId = tagId;
		Priority = priority;
		VerticalOffset = verticalOffset;
	}

	public AimBehaviourTagOptions(Id tagId, Domain.Model.Profiles.Behaviours.AimBehaviour.TagOptions options)
	{
		TagId = tagId;
		Priority = options.Priority;
		VerticalOffset = options.VerticalOffset;
	}
}