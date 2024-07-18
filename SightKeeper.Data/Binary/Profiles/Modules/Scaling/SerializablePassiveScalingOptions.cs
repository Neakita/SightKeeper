using MemoryPack;
using SightKeeper.Domain.Model.Profiles.Modules.Scaling;

namespace SightKeeper.Data.Binary.Profiles.Modules.Scaling;

[MemoryPackable]
[MemoryPackUnion(0, typeof(SerializableConstantScalingOptions))]
[MemoryPackUnion(1, typeof(SerializableIterativeScalingOptions))]
internal abstract partial class SerializablePassiveScalingOptions
{
	public abstract PassiveScalingOptions Convert();
}