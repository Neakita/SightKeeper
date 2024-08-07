using MemoryPack;

namespace SightKeeper.Data.Binary.Profiles.Modules.Scaling;

[MemoryPackable]
[MemoryPackUnion(0, typeof(ConstantScalingOptions))]
[MemoryPackUnion(1, typeof(IterativeScalingOptions))]
internal abstract partial class PassiveScalingOptions
{
	public abstract Domain.Model.Profiles.Modules.Scaling.PassiveScalingOptions Convert();
}