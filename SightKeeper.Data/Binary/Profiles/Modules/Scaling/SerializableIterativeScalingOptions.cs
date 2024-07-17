using MemoryPack;
using SightKeeper.Domain.Model.Profiles.Modules.Scaling;

namespace SightKeeper.Data.Binary.Profiles.Modules.Scaling;

[MemoryPackable]
internal sealed partial class SerializableIterativeScalingOptions : SerializablePassiveScalingOptions
{
	public float MinimumScale { get; }
	public float MaximumScale { get; }

	[MemoryPackConstructor]
	public SerializableIterativeScalingOptions(float minimumScale, float maximumScale)
	{
		MinimumScale = minimumScale;
		MaximumScale = maximumScale;
	}

	public SerializableIterativeScalingOptions(IterativeScalingOptions options)
	{
		MinimumScale = options.MinimumScaling;
		MaximumScale = options.MaximumScaling;
	}
}