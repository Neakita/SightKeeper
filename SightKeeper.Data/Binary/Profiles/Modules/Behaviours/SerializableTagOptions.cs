using FlakeId;
using MemoryPack;

namespace SightKeeper.Data.Binary.Profiles.Modules.Behaviours;

[MemoryPackable]
public sealed partial class SerializableTagOptions
{
	public Id TagId { get; set; }
	public byte Priority { get; set; }
	public float VerticalOffset { get; set; }
}