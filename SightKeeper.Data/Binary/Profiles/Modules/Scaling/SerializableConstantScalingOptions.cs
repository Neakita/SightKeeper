using MemoryPack;
using SightKeeper.Domain.Model.Profiles.Modules.Scaling;

namespace SightKeeper.Data.Binary.Profiles.Modules.Scaling;

[MemoryPackable]
internal sealed partial class SerializableConstantScalingOptions : SerializablePassiveScalingOptions
{
	public float Factor { get; }

	[MemoryPackConstructor]
	public SerializableConstantScalingOptions(float factor)
	{
		Factor = factor;
	}

	public SerializableConstantScalingOptions(ConstantScalingOptions options)
	{
		Factor = options.Factor;
	}
}