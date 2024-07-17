using MemoryPack;
using SightKeeper.Domain.Model.Profiles.Modules.Scaling;

namespace SightKeeper.Data.Binary.Profiles.Modules.Scaling;

[MemoryPackable]
internal sealed partial class SerializableAdaptiveScalingOptions : SerializableActiveScalingOptions
{
	public float Margin { get; }
	public float MaximumScaling { get; }

	[MemoryPackConstructor]
	public SerializableAdaptiveScalingOptions(float margin, float maximumScaling)
	{
		Margin = margin;
		MaximumScaling = maximumScaling;
	}

	public SerializableAdaptiveScalingOptions(AdaptiveScalingOptions options)
	{
		Margin = options.Margin;
		MaximumScaling = options.MaximumScaling;
	}
}