using FlakeId;
using MemoryPack;

namespace SightKeeper.Data.Binary.Profiles.Modules;

[MemoryPackable]
[MemoryPackUnion(0, typeof(SerializableDetectorModule))]
[MemoryPackUnion(1, typeof(SerializableClassifierModule))]
[MemoryPackUnion(2, typeof(SerializablePoserModule))]
public abstract partial class SerializableModule
{
	public Id WeightsId { get; set; }
	public Scaling.SerializablePassiveScalingOptions? PassiveScalingOptions { get; set; }
	public Walking.SerializablePassiveWalkingOptions? PassiveWalkingOptions { get; set; }
}