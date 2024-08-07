using MemoryPack;

namespace SightKeeper.Data.Binary.Profiles.Modules.Scaling;

[MemoryPackable]
internal sealed partial class ConstantScalingOptions : PassiveScalingOptions
{
	public float Factor { get; }

	[MemoryPackConstructor]
	public ConstantScalingOptions(float factor)
	{
		Factor = factor;
	}

	public ConstantScalingOptions(Domain.Model.Profiles.Modules.Scaling.ConstantScalingOptions options)
	{
		Factor = options.Factor;
	}

	public override Domain.Model.Profiles.Modules.Scaling.ConstantScalingOptions Convert()
	{
		return new Domain.Model.Profiles.Modules.Scaling.ConstantScalingOptions(Factor);
	}
}