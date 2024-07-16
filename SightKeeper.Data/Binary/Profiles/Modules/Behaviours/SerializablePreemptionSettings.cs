using MemoryPack;
using SightKeeper.Domain.Model;

namespace SightKeeper.Data.Binary.Profiles.Modules.Behaviours;

[MemoryPackable]
public sealed partial class SerializablePreemptionSettings
{
	public Vector2<double> Factor { get; set; }
}