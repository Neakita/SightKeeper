using MemoryPack;

namespace SightKeeper.Data.Binary.Profiles.Modules.Scaling;

[MemoryPackable]
internal sealed partial class ScalingOptions : ActiveScalingOptions
{
	public float Margin { get; }
	public float MaximumScaling { get; }

	[MemoryPackConstructor]
	public ScalingOptions(float margin, float maximumScaling)
	{
		Margin = margin;
		MaximumScaling = maximumScaling;
	}

	public ScalingOptions(Domain.Model.Profiles.Modules.Scaling.AdaptiveScalingOptions options)
	{
		Margin = options.Margin;
		MaximumScaling = options.MaximumScaling;
	}
}