using FlakeId;
using MemoryPack;
using SightKeeper.Domain.Model.Profiles.Behaviours;

namespace SightKeeper.Data.Binary.Profiles.Modules.Behaviours;

[MemoryPackable]
internal sealed partial class SerializableAimBehaviourTagOptions
{
	public Id TagId { get; set; }
	public byte Priority { get; set; }
	public float VerticalOffset { get; set; }

	[MemoryPackConstructor]
	public SerializableAimBehaviourTagOptions(Id tagId, byte priority, float verticalOffset)
	{
		TagId = tagId;
		Priority = priority;
		VerticalOffset = verticalOffset;
	}

	public SerializableAimBehaviourTagOptions(Id tagId, AimBehaviour.TagOptions options)
	{
		TagId = tagId;
		Priority = options.Priority;
		VerticalOffset = options.VerticalOffset;
	}
}