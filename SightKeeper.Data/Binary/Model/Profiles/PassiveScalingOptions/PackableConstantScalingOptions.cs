using MemoryPack;
using SightKeeper.Domain.Model.Profiles.Modules.Scaling;

namespace SightKeeper.Data.Binary.Model.Profiles.PassiveScalingOptions;

/// <summary>
/// MemoryPackable version of <see cref="ConstantScalingOptions"/>
/// </summary>
[MemoryPackable]
internal sealed partial class PackableConstantScalingOptions : PackablePassiveScalingOptions
{
	public float Factor { get; }

	public PackableConstantScalingOptions(float factor)
	{
		Factor = factor;
	}
}