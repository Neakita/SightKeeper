using FlakeId;
using MemoryPack;

namespace SightKeeper.Data.Binary.Profiles.Modules.Behaviours;

[MemoryPackable]
internal sealed partial class SerializableAimBehaviourTagOptions
{
	public Id TagId { get; set; }
	public byte Priority { get; set; }
	public float VerticalOffset { get; set; }
}